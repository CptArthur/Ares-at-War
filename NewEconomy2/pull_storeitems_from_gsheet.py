import os
import requests
import xml.etree.ElementTree as ET
import xml.dom.minidom

from datetime import datetime
from bs4 import BeautifulSoup


SPREADSHEETS = {'StoreItemsContainer': ['1T01zkz0LdFaAv-Ojuy1Mzi8SkQ7c1ETBHYqaqPH0Bj4', ['0']]}
MOD = 'MG'

OUTPUT_DIR = os.path.join(os.path.dirname(os.path.dirname(__file__)), 'Content', 'Data', 'StoreItems')


def scrape_spreadsheet(spreadsheet, grid) -> list:
    spreadsheet_url = 'https://docs.google.com/spreadsheets/d/' + spreadsheet[0] + '/gviz/tq?tqx=out:html&tq&gid=' + grid

    html = requests.get(spreadsheet_url).text
    soup = BeautifulSoup(html, 'lxml')

    salas_cine = soup.find_all('table')[0]
    rows = [[td.text for td in row.find_all("td")] for row in salas_cine.find_all('tr')]

    formatted_list = []
    for row in rows:
        if row[0] in ['']:
            continue

        row_list = []
        header_list = []
        for idx, cell in enumerate(row):
            if row[0] == '*Internal* Reference':
                cell = cell.replace(" ", "")

            if cell.startswith('*'):
                end = cell[1:].find('*')
                extracted = cell[1:end+1]

                offset = len(header_list) + 2 - idx

                for i in range(0,offset):
                    header_list.append(None)

                header_list.append(extracted)


            if cell == '\xa0' or cell == '':
                row_list.append(None)

            elif cell == '✔':
                row_list.append(True)

            elif cell == '✗':
                row_list.append(False)

            elif cell[0] == '*':
                end = cell[1:].find('*')
                if cell[end+3:] == '':
                    row_list.append(cell[1:end+1])
                else:
                    row_list.append(cell[end+2:])

            else:
                row_list.append(cell.replace(",",""))

        formatted_list.append(row_list)

    return formatted_list


def reorganize_table_to_dict(rows) -> dict:

    del rows[0]

    entries = {}
    container_name = ""
    item_id = ""

    for row in rows:
        if row is None:
            continue

        if container_name == "":
            container_name = row[0]
            entries[container_name] = {}
        elif row[0] != container_name:
            container_name = row[0]
            entries[container_name] = {}

        if item_id == "":
            item_id = row[1]
            entries[container_name][item_id] = {}
        elif row[1] != item_id:
            item_id = row[1]
            entries[container_name][item_id] = {}

        entries[container_name][item_id]["ItemType"] = row[2]

        if "ItemSubtypeIds" not in entries[container_name][item_id]:
            entries[container_name][item_id]["ItemSubtypeIds"] = []
        else:
            entries[container_name][item_id]["ItemSubtypeIds"].append(row[3])

        entries[container_name][item_id]["Offer_MinPriceMultiplier"] = row[4]
        entries[container_name][item_id]["Offer_MaxPriceMultiplier"] = row[5]
        entries[container_name][item_id]["Offer_MinAmount"] = row[6]
        entries[container_name][item_id]["Offer_MaxAmount"] = row[7]

        entries[container_name][item_id]["Order_MinPriceMultiplier"] = row[8]
        entries[container_name][item_id]["Order_MaxPriceMultiplier"] = row[9]
        entries[container_name][item_id]["Order_MinAmount"] = row[10]
        entries[container_name][item_id]["Order_MaxAmount"] = row[11]

    return entries


def write_dialoguebank(filetype, rows):

    entries = reorganize_table_to_dict(rows)

    for container_name, container_content in entries.items():
        container = ET.Element('StoreItemsContainer')
        container.set('xmlns:xsd', 'http://www.w3.org/2001/XMLSchema')
        container.set('xmlns:xsi', 'http://www.w3.org/2001/XMLSchema-instance')

        items = ET.SubElement(container, 'StoreItems')

        for id, id_content in container_content.items():
            item = ET.SubElement(items, 'StoreItem')

            item_id = ET.SubElement(item, 'StoreItemId')
            item_id.text = id

            item_type = ET.SubElement(item, 'ItemType')
            item_type.text = id_content["ItemType"]

            item_subtypes = ET.SubElement(item, 'ItemSubtypeIds')

            for item_subtypeid in id_content["ItemSubtypeIds"]:
                item_subtype = ET.SubElement(item_subtypes, 'ItemSubtypeId')
                item_subtype.text = item_subtypeid

            offer = ET.SubElement(item, 'Offer')
            offer_minprice = ET.SubElement(offer, 'MinPriceMultiplier')
            offer_minprice.text = id_content["Offer_MinPriceMultiplier"]
            offer_maxprice = ET.SubElement(offer, 'MaxPriceMultiplier')
            offer_maxprice.text = id_content["Offer_MaxPriceMultiplier"]
            offer_minamount = ET.SubElement(offer, 'MinAmount')
            offer_minamount.text = id_content["Offer_MinAmount"]
            offer_maxamount = ET.SubElement(offer, 'MaxAmount')
            offer_maxamount.text = id_content["Offer_MaxAmount"]

            order = ET.SubElement(item, 'Order')
            order_minprice = ET.SubElement(order, 'MinPriceMultiplier')
            order_minprice.text = id_content["Order_MinPriceMultiplier"]
            order_maxprice = ET.SubElement(order, 'MaxPriceMultiplier')
            order_maxprice.text = id_content["Order_MaxPriceMultiplier"]
            order_minamount = ET.SubElement(order, 'MinAmount')
            order_minamount.text = id_content["Order_MinAmount"]
            order_maxamount = ET.SubElement(order, 'MaxAmount')
            order_maxamount.text = id_content["Order_MaxAmount"]


        temp_string = ET.tostring(container, 'utf-16')
        temp_string.decode('utf-16')
        xml_string = xml.dom.minidom.parseString(temp_string)
        xml_formatted = xml_string.toprettyxml()
        xml_formatted = xml_formatted.replace('<?xml version="1.0" ?>', f'<?xml version="1.0" encoding="utf-16"?>\n\n<!-- Created from GSheet export on {datetime.now()} -->\n')
        xml_formatted = xml_formatted.replace("<StoreItems>","<StoreItems>\n")
        xml_formatted = xml_formatted.replace("</StoreItem>","</StoreItem>\n")

        target_file = os.path.join(OUTPUT_DIR, f"{container_name}.xml")
        exported_xml = open(target_file, "w")
        exported_xml.write(xml_formatted)


def main():

    for key, val in SPREADSHEETS.items():
        write_dialoguebank(key, scrape_spreadsheet(val, '230306071'))


if __name__ == '__main__':
    main()