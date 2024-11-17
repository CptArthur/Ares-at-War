import os

def replace_text_in_files(directory, replacement_text):
    # Walk through the directory and all subdirectories
    for root, _, files in os.walk(directory):
        for filename in files:
            # Check if the file has a .sbc extension
            if filename.endswith(".sbc"):
                filepath = os.path.join(root, filename)
                
                # Open the file and read its contents with UTF-8 encoding
                try:
                    # Open the file and read its contents
                    with open(filepath, 'r', encoding='utf-8') as file:
                        file_content = file.read()
                    
                    # Replace {FAC} with the replacement text
                    new_content = file_content.replace("FAC-Shipyard-Shipyard", "AaW_Shipyard_MainProfile")
                    
                    # Write the updated content back to the file
                    with open(filepath, 'w', encoding='utf-8') as file:
                        file.write(new_content)
                    
                    print(f"Updated: {filepath}")
                    
                except UnicodeDecodeError as e:
                    print(f"Error reading {filepath}: {e}")

# Get the name of the current Python script, without the .py extension
current_script_name = os.path.splitext(os.path.basename(__file__))[0]

# Replace `.` with your specific directory if needed
replace_text_in_files('.', current_script_name)
input()