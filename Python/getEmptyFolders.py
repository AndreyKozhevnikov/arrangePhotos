import os,csv

with open("c:\Dropbox\C#\RenamePhotos\MyCSVExport.csv",'w+',newline='',encoding="utf-8") as f:
  w=csv.writer(f,delimiter = ";")
  root_fld_name=r'd:\photo'
  #root_fld_name=r'd:\temp'
  for path,dirs,  files in os.walk(root_fld_name):
      if len(files)==0 and len(dirs)==0 :
        print(path)
        print(files)
     
