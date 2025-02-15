import requests


def GetCSV(name,gid):
    # Google Sheet ID and GID
    sheet_id = '1p7DlFDqk5RXT1X9ijevAVTMLepMki8pFHacYmYaVKrQ'

    # Construct the CSV download URL
    csv_url = f'https://docs.google.com/spreadsheets/d/{sheet_id}/export?format=csv&id={sheet_id}&gid={gid}'

    # Send GET request to download the CSV
    response = requests.get(csv_url)

    # Check if the request was successful
    if response.status_code == 200:
        # Save the content to a file
        with open(f'{name}.csv', 'wb') as file:
            file.write(response.content)
        print('CSV downloaded successfully.')
    else:
        print('Failed to download CSV. Status code:', response.status_code)

GetCSV("XML","0")
GetCSV("Encounters","327410979")
input()