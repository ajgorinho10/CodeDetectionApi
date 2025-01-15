import base64
import requests
import json

def convert_image_to_base64(image_path):
    with open(image_path, "rb") as image_file:
        image_bytes = image_file.read()
        base64_string = base64.b64encode(image_bytes).decode('utf-8')
        return base64_string
    
def send_image_to_api(base64_image):
    base64_image = convert_image_to_base64(image_path)
    url = "https://localhost:7045/Detect/bar"

    data = {
        "img": base64_image
    }

    response = requests.post(url, json=data, verify=False)

    if response.status_code == 200:
        response.content
        print(response.content)
    else:
        print(f"Failed to upload image. Status code: {response.status_code}")

image_path = "./kodyBarZdjecia/bar_16.jpg"
base64_image = convert_image_to_base64(image_path)
send_image_to_api(base64_image)