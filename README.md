#Echo's TODO
- add A way to prevent all changes until validated for a new patch.
- add dev override for that stop so we can actually do fixes.
- add more sheets.
- Try to find a way to make adding new values and sheets not a struggle.
- add handling for Strings(CHECK NAMINGWAY) and packedbools
- Add codepath to create custom entries(reference field, edited fields). Saved without indexes and dynamically updated.
- Redo readme
- Automatically generate/update mod to enable default paths for stuff.
- More graceful configfile handling to allow for it to handle changes in data.
- When Saving Data, Only save edited values.
- Fix config loading to skip unknown sets.
- Add some sort of config verification?

#taco's to-do:
- limit input values to prevent the log from going haywire
- refactoring broke sheet calls when reenabling the plugin somehow. need to fix it
- figure out how to streamline some of the fields
- more sheets to break stuff
- find out how to circumvent handling of the packed bools
- checkmarks to enable sheets don't actually make a difference right now


`FieldOffset(0x??)` is the actual address being updated. Lumina sheets are just used for pulling base data. so, technically, if the column doesn't exist in the source data, a dummy field can be supplied, although that does make things slightly obtuse.


# ActionTimelineReplacement, a.k.a. ATR

Special credit to Loskh for the original development, and to DethiCra for completely rebuilding the plugin, as well as creating the interface.


Use `/atr` to access the config window quicker.

## Q&A

### Can these changes be detected by the server?
Mostly no, but also sort of yes because I like being bad. If it's especially dangerous and it's clearly labelled as such, it won't be included because there's no reason to go there.
