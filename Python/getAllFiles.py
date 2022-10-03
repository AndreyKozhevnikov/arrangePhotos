import os,csv

with open(r"c:\temp\MyCSVExport.csv",'w+',newline='',encoding="utf-8") as f:
  w=csv.writer(f,delimiter = ";")
  root_fld_name=r'f:\photo'
  #root_fld_name=r'd:\temp'
  for path,tst,  files in os.walk(root_fld_name):
     for filename in files:
        fullName=os.path.join(path,filename)
        info = os.stat(fullName)
        size=info.st_size
        ctime=info.st_mtime
        fullData=[fullName,filename,ctime,size]
        w.writerow(fullData)
     print(path)
      #print(path)
      #print(filename)
      #print(size)
      #print(ctime)
      #print(info)
      #print(path+filename)
