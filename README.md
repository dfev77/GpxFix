# GpxFix

Add time to gpx track in order to allow it to be imported into https://connect.garmin.com.

The tool works by adding `time` nodes to all track point elements. Time starts from 1900/1/1 and is incremented with 1s for each node, so it will look like you are FAST! :).

Usage: GpxFix.exe \<gpx file path\>
