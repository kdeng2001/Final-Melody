INCLUDE ../GLOBALS.ink

{instrument_name == "": -> main | -> already_chosen}
=== main ===
Pick an instrument kid. #speaker:mum #layout:left
    + [Keytar]
        -> chosen("Keytar")
    + [Guitar]
        -> chosen("Guitar")
    + [Drums]
        -> chosen("Drums")
=== chosen(instrument) ===
~instrument_name = instrument
Take this {instrument}.
-> END

=== already_chosen ===
You already chose {instrument_name}. #speaker:mum #layout:left
What more could you possibly want?
-> END