import requests
import xml.etree.ElementTree as ET

# Path to the modinfo.sbmi file
modinfo_path = "modinfo.sbmi"

# Parse the XML file to get the Workshop ID
try:
    tree = ET.parse(modinfo_path)
    root = tree.getroot()
    
    # Find the Workshop ID within <WorkshopIds>
    workshop_id = root.find(".//WorkshopIds/WorkshopId/Id").text
    print(f"Using Workshop ID: {workshop_id}")

except FileNotFoundError:
    print("Error: modinfo.sbmi file not found.")
    sys.exit(1)
except AttributeError:
    print("Error: Workshop ID not found in modinfo.sbmi.")
    sys.exit(1)

# Steam API URL for getting Workshop item details
steam_api_url = "https://api.steampowered.com/ISteamRemoteStorage/GetPublishedFileDetails/v1/"
params = {
    "itemcount": 1,
    "publishedfileids[0]": workshop_id
}

# Read the local patch notes
try:
    with open("patch_notes.md", "r") as file:
        patch_notes = file.read()
        patch_notes = patch_notes[:1024] + "..." if len(patch_notes) > 1024 else patch_notes
except FileNotFoundError:
    patch_notes = "No patch notes available."

# Fetch details from Steam API
steam_response = requests.post(steam_api_url, data=params)
if steam_response.status_code == 200:
    item_data = steam_response.json()['response']['publishedfiledetails'][0]
    
    mod_title = item_data.get('title', 'Unknown Title')
    mod_url = f"https://steamcommunity.com/sharedfiles/filedetails/?id={workshop_id}"
    mod_thumbnail_url = item_data.get('preview_url', '')

    # Define Discord webhook URL
    discord_webhook_url = "https://discord.com/api/webhooks/1299401523524272169/BdWyx3bRlk6lFlav8km88sGM3n6-8jlrya9MfuNaVKJik6oQ5kL52k0teuUFxn9QR_DK"

    # Discord payload with an embed
    discord_data = {
        "embeds": [
            {
                "title": f"{mod_title} - Updated!",
                "description": f"[View on Steam Workshop]({mod_url})",
                "color": 3066993,
                "fields": [
                    {
                        "name": "Patch Notes",
                        "value": patch_notes,
                        "inline": False
                    }
                ],
                "thumbnail": {
                    "url": mod_thumbnail_url
                },
                "footer": {
                    "text": "Steam Workshop Mod Update"
                }
            }
        ]
    }

    # Send the embed message via POST request
    response = requests.post(discord_webhook_url, json=discord_data)
    if response.status_code == 204:
        print("Mod update embed sent successfully!")
    else:
        print(f"Failed to send embed. Status code: {response.status_code}, Response: {response.text}")
else:
    print(f"Failed to fetch Steam Workshop data. Status code: {steam_response.status_code}")
