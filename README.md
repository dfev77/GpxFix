# GpxFix

Add time to gpx track in order to allow it to be imported into https://connect.garmin.com, as gpx tracks with no time information cannot be loaded.

Usage: GpxFix.exe \<gpx file path or folder\>

It will load the provided file or loop all .gpx files in the provided folder and output a file named `<input file name>_updated.gpx` with time fields added.
The tool works by adding `time` nodes to all track point elements, starting from current UTC time and adding 1s for each track point.