import os
root_fld_name=r'd:\temp'
fld_name=r"ss27-09-2020_09-50-06"
fld_newName=r"ss27-09-2020_09-50-06test"
path1=os.path.join(root_fld_name,fld_name)
path2=os.path.join(root_fld_name,fld_newName)
print(path1)
print(path2)
if os.path.exists(path1):
  os.rename(path1,path2)
  print('success')
else:
    print('fail')
