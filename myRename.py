csv_name=r"d:\Dropbox\C#\RenamePhotos\names.csv"


import csv
import os

with open(csv_name,newline='', encoding="utf8") as f:
    reader=csv.reader(f, delimiter=';')

    data=list(reader)
root_fld_name=r'd:\photo'
for row in data:
    oldName=row[0]
    newName=row[1]
    oldPath=os.path.join(root_fld_name,oldName)
    newPath=os.path.join(root_fld_name,newName)
   # if not os.path.exists(oldPath):
    #    print('fail'+oldPath)
    if os.path.exists(oldPath):
        os.rename(oldPath,newPath)
        print('ok '+newPath)
   # print(oldPath)
   # print(newPath)
   # print(os.path.exists(oldPath))
    #print('----')
  #for column in row:
    #print (row[0])
   # print (row[1])
  
#print(data)
