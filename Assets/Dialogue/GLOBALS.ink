VAR instrument_name = ""
VAR player_name = "Anonymous"

EXTERNAL get_name()
=== function get_name() ===
~ return player_name

// place at start of any color text
CONST redStart = "<color=\#E81416>"
CONST orangeStart = "<color=\#FFA500>"
CONST yellowStart = "<color=\#FAEB36>"
CONST greenStart = "<color=\#79C314>"
CONST blueStart = "<color=\#487DE7>"
CONST indigoStart = "<color=\#4B369D>"
VAR violetStart = "<color=\#70369d>"

// place at end of any color text
CONST colorEnd = "</color>"