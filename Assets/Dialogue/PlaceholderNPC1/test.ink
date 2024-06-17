-> main

=== main ===
// line 1
Would you like to buy some <b><color=\#CB1F3F>squirrels</color></b> young human? Ehehehehehehe... #speaker:Old Lady? #portrait:old_lady #layout:right

// line 2
Suddenly, the old lady procures a stash of <b><color=\#CB1F3F>squirrels</color></b> from her magic hat and lays them out on a sheet of blanket, showcasing her wares. #layout:default

How about it lad? #layout: right

    * [Yes]
        -> choice0
    * [No]
        -> choice1
    * -> 
    I hope you like your squirrel.
-> END
=== choice0 ===
Excellent choice. 
You received a <b><color=\#CB1F3F>squirrel</color></b>! #layout:default
-> END
=== choice1 ===
Now, now, don't be shy.
<b><color=\#CB1F3F>Food</color></b> doesn't come by easy these days.
You received a <b><color=\#CB1F3F>squirrel</color></b>! #layout:default
-> END
