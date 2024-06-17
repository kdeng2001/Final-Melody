INCLUDE ../GLOBALS.ink

-> main

=== main ===
Ooh, well welcome {player_name}! #speaker:???
I am the divine god of music.
I beseech your aid.
The musical world is collapsing.
Choose your instrument and save the world...
    + [Guitar]
        -> chosen("Guitar")
    + [Keytar]
        -> chosen("Keytar")
    + [Drums]
        -> chosen("Drums")

=== chosen (instrument) ===
~instrument_name = instrument
Now go, {player_name}.
Wield the power of your {instrument_name}.
Save the world of music...
->END