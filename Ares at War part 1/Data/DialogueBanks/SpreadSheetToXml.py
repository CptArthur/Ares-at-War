import requests
import csv
import xml.etree.ElementTree as ET
from xml.dom import minidom

# Step 1: Download the CSV from Google Sheets
sheet_id = "18CvmC2K07UxEYQxK5uEbW7qU4z8NggYSSlHgBAQ8ZgE"
gid = "0"
csv_url = f"https://docs.google.com/spreadsheets/d/{sheet_id}/export?format=csv&id={sheet_id}&gid={gid}"

response = requests.get(csv_url)
csv_content = response.content.decode('utf-8')

# Step 2: Parse the CSV
rows = list(csv.reader(csv_content.splitlines()))

# Step 3: Create the XML structure
root = ET.Element("DialogueBank", {
    'xmlns:xsd': "http://www.w3.org/2001/XMLSchema",
    'xmlns:xsi': "http://www.w3.org/2001/XMLSchema-instance"
})

# Add the ChatProfileId element
chat_profile_id = ET.SubElement(root, "ChatProfileId")
chat_profile_id.text = "MSB_DialogueBank_Chat"

# Group the rows by DialogueCueId (assuming first row is header)
dialogue_cues = {}
for row in rows[1:]:  # Skip the header row
    dialogue_cue_id, text, audio_id, weight = row
    if dialogue_cue_id not in dialogue_cues:
        dialogue_cues[dialogue_cue_id] = []
    dialogue_cues[dialogue_cue_id].append({
        'Text': text,
        'AudioId': audio_id,
        'Weight': weight
    })

# Create XML structure based on DialogueCueId
for cue_id, entries in dialogue_cues.items():
    dialogue_cue = ET.SubElement(root, "DialogueCue")
    dialogue_cue_id_elem = ET.SubElement(dialogue_cue, "DialogueCueId")
    dialogue_cue_id_elem.text = cue_id

    entries_elem = ET.SubElement(dialogue_cue, "Entries")
    
    for entry in entries:
        entry_inner = ET.SubElement(entries_elem, "Entry")
        
        message_elem = ET.SubElement(entry_inner, "Message")
        message_elem.text = entry['Text']

        audio_id_elem = ET.SubElement(entry_inner, "AudioId")
        audio_id_elem.text = entry['AudioId']

        weight_elem = ET.SubElement(entry_inner, "Weights")
        weight_elem.text = str(entry['Weight'])

# Step 4: Prettify and save the XML
def prettify(elem):
    rough_string = ET.tostring(elem, 'utf-8')
    reparsed = minidom.parseString(rough_string)
    return reparsed.toprettyxml(indent="  ")

# Generate and save the final XML
xml_str = prettify(root)
with open("MSB.xml", "w", encoding="utf-16") as f:
    f.write(xml_str)

print("XML created and saved as dialogue_bank.xml")
input()
