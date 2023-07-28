=== SecondRoom ===
#scene:second_room.tscn

- (top)
# objects
<-  Gem
<-  Key
<-  Door
-   -> DONE



= Gem
    +   {second_room has gem} [-- Gem]
        { FirstRoom.what_needs_cube2.guessed_it:
            You take the gem.
            ~ move(gem, second_room, inventory)
        - else:
            Why should you take this gem?
        } <>
    -   -> top



= Key
    +   {second_room has key} [-- Key]
        { FirstRoom.what_needs_cube1.guessed_it:
            You grab the key.
            ~ move(key, second_room, inventory)
        - else:
            Why should you take this key? <>
        } <>
    -   -> top



= Door
    +   [-- Door]
        You open the door to the first room and walk forth.
        -> go_to(-> FirstRoom)
    -   -> top