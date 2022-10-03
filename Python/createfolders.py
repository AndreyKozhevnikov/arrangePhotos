csv_name=r"d:\Dropbox\C#\RenamePhotos\names2.csv"

import csv
import os

with open(csv_name,newline='', encoding="utf8") as f:
    reader=csv.reader(f, delimiter=';')

    data=list(reader)
root_fld_name=r'd:\photo'
for row in data:
    newName=os.path.join(root_fld_name,row[0])
    if not os.path.exists(newName):
      os.mkdir(newName)
      print('ok '+newName)
    else:
      print('fail '+newName)
