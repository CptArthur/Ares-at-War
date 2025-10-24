import os
from PIL import Image
import shutil

# Path to your folder
folder = r"C:\Users\maart\AppData\Roaming\SpaceEngineers\Mods\Ares at War Food\Icons\Crops"

# Template files
food_template = os.path.join(folder, "FoodContainer_Empty.dds")
seed_template = os.path.join(folder, "SeedPack_Empty.dds")

def overlay_png_on_dds(template_path, png_path, target_path):
    # Open template and PNG
    template = Image.open(template_path).convert("RGBA")
    png = Image.open(png_path).convert("RGBA")
    
    # Calculate position for bottom-right overlay
    template_width, template_height = template.size
    square_width = template_width // 2
    square_height = template_height // 2

    # Resize PNG to fit the square
    png = png.resize((square_width, square_height), Image.Resampling.LANCZOS)

    # Bottom-right corner position
    position = (square_width, square_height)
    
    # Paste the PNG onto the template
    template.paste(png, position, mask=png)
    
    # Save as DDS
    template.save(target_path)
    print(f"Created overlay: {target_path}")


# Loop through all files in the folder
for file in os.listdir(folder):
    if file.lower().endswith(".png"):
        base_name = os.path.splitext(file)[0]
        png_path = os.path.join(folder, file)
        
        # Food container
        food_target = os.path.join(folder, f"FoodContainer_{base_name}.dds")
        if not os.path.exists(food_target):
            overlay_png_on_dds(food_template, png_path, food_target)
        
        # Seed pack
        seed_target = os.path.join(folder, f"SeedPack_{base_name}.dds")
        if not os.path.exists(seed_target):
            overlay_png_on_dds(seed_template, png_path, seed_target)
