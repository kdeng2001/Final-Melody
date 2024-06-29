INCLUDE ../GLOBALS.ink

-> main

=== main ===
Wake up, {player_name}. #speaker:mum #layout:right #portrait: mom_default
You can't sleep all day, I have a surprise for your birthday!
({player_name} gets out of bed and Mom gives {player_name} their {instrument_name}.) #layout:default

{instrument_name == "Keytar": -> keytar_guitar}
{instrument_name == "Guitar": -> keytar_guitar}
{instrument_name == "Drums": -> drums}
-> DONE
=== keytar_guitar ===
You know when I was a kid, I always played the {instrument_name}. #speaker:mum #layout:right #portrait: mom_default
You obtained the {instrument_name}!
It seems oddly similar to the one in that dream of yours. #layout:default
You tell mom that.
Oh? Had a dream about this {instrument_name}? #speaker: mum #layout:right #portrait: mom_default
Well you must really be excited to play then.
->END

=== drums ===
You know when I was a kid, I always played the {instrument_name}. #speaker:mum #layout:right #portrait: mom_default
You obtained the {instrument_name}!
It seems oddly similar to the one in that dream of yours. #layout:default
You tell mom that.
Oh? Had a dream about these {instrument_name}? #speaker: mum #layout:right #portrait: mom_default
Well you must really be excited to play then.
-> END