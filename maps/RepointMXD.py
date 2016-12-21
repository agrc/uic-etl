# repoint mxds at dev or AT Database
# requires that you have database connections in
# catalog with the names below
import arcpy
import glob
import sys
from os.path import abspath
from os.path import dirname
from os.path import join


db = join(abspath(dirname(__file__)), '..', 'data')
at = join(db, 'stage.sde')
prod = join(db, 'prod.sde')
target = None

if len(sys.argv) > 1:
    target = sys.argv[1]

if not target:
    target = raw_input('AT (A), or Prod (P)? ')

if target == 'A':
    dest = at
elif target == 'P':
    dest = prod

print('updating mxd\'s to use {}'.format(dest))

for f in glob.glob('*.local.mxd'):
    print(f)
    mxd = arcpy.mapping.MapDocument(f)
    l = arcpy.mapping.ListLayers(mxd)[0]
    mxd.findAndReplaceWorkspacePaths(l.workspacePath, dest, True)
    mxd.save()

print('done')
