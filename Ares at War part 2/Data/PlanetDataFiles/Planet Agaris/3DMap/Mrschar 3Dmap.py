import numpy as np
from PIL import Image
import os

def extract_faces_from_cross_layout(image):
    """Extract the individual cubemap faces from a cross-layout image."""
    width, height = image.size
    face_size = width // 4  # Assuming a standard cross layout

    # Extract faces using coordinates
    faces = {
        "left": image.crop((0, face_size, face_size, 2 * face_size)),
        "front": image.crop((face_size, face_size, 2 * face_size, 2 * face_size)),
        "right": image.crop((2 * face_size, face_size, 3 * face_size, 2 * face_size)),
        "back": image.crop((3 * face_size, face_size, 4 * face_size, 2 * face_size)),
        "top": image.crop((face_size, 0, 2 * face_size, face_size)),
        "bottom": image.crop((face_size, 2 * face_size, 2 * face_size, 3 * face_size))
    }

    return faces

def sample_cubemap(cubemap_images, theta, phi):
    """Sample the cubemap for the given spherical coordinates."""
    x = np.sin(phi) * np.cos(theta)
    y = np.sin(phi) * np.sin(theta)
    z = np.cos(phi)

    abs_x = abs(x)
    abs_y = abs(y)
    abs_z = abs(z)

    if abs_x >= abs_y and abs_x >= abs_z:
        if x > 0:
            face = "right"
            u = -z / abs_x
            v = -y / abs_x
        else:
            face = "left"
            u = z / abs_x
            v = -y / abs_x
    elif abs_y >= abs_x and abs_y >= abs_z:
        if y > 0:
            face = "top"
            u = x / abs_y
            v = z / abs_y
        else:
            face = "bottom"
            u = x / abs_y
            v = -z / abs_y
    else:
        if z > 0:
            face = "front"
            u = x / abs_z
            v = -y / abs_z
        else:
            face = "back"
            u = -x / abs_z
            v = -y / abs_z

    u = (u + 1) / 2.0
    v = (v + 1) / 2.0

    face_img = cubemap_images[face]
    width, height = face_img.size
    pixel_x = int(u * (width - 1))
    pixel_y = int(v * (height - 1))

    return face_img.getpixel((pixel_x, pixel_y))

def cubemap_to_equirectangular(cubemap_images, output_size):
    width, height = output_size
    equirect_img = Image.new("RGB", (width, height))

    for y in range(height):
        phi = np.pi * (y / height)
        for x in range(width):
            theta = 2 * np.pi * (x / width)
            color = sample_cubemap(cubemap_images, theta, phi)
            equirect_img.putpixel((x, y), color)

    return equirect_img

def main():
    input_path = input("Enter the path to the cubemap cross-layout PNG image: ")
    if not os.path.exists(input_path):
        print(f"Error: {input_path} not found.")
        return

    input_image = Image.open(input_path).convert("RGB")
    cubemap_images = extract_faces_from_cross_layout(input_image)

    output_size = (2048, 1024)

    equirect_img = cubemap_to_equirectangular(cubemap_images, output_size)

    output_path = os.path.join(os.path.dirname(input_path), "equirectangular_output.png")
    equirect_img.save(output_path)
    print(f"Equirectangular image saved at: {output_path}")

if __name__ == "__main__":
    main()
