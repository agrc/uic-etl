import arcpy
from collections import defaultdict

print('connecting to uic')
arcpy.env.workspace = 'C:\\Projects\\GitHub\\uic-etl\\data\\prod.sde'
facility_lookup = defaultdict(list)
contact_lookup = {}

print('finding all contacts')
with arcpy.da.SearchCursor('UICContact', ['Guid', 'ContactType']) as contact_cursor:
    for id, type in contact_cursor:
        contact_lookup[id] = str(type)

print('building relationship')
with arcpy.da.SearchCursor('UICFacilityToContact', ['FacilityGUID', 'ContactGUID']) as related_cursor:
    for facility, contact in related_cursor:
        try:
            person = contact_lookup[contact]
            facility_lookup[facility].append(person)
        except KeyError:
            continue

#: print the facilities that have more than one top level contact
for id, contacts in facility_lookup.iteritems():
    count = contacts.count('1')
    if count > 1:
        print('{} has {} contacts of type {}'.format(id, count, ','.join(contacts)))
