=== FirstRoom ===
#scene:first_room.tscn

- (top)
# objects
<-  Cube1
<-  Cube2
<-  Key
<-  Gem
<-  Door
<-  win_check
-   -> DONE



= Door
    +   [-- Door]
        You cross the threshold to the second room with hesitation.
        -> go_to(-> SecondRoom)
    -   -> top



= Cube1
    +   [-- Cube1]
        You look at the left cube.
        {
            - not what_needs_cube1.guessed_it:
                -> what_needs_cube1 ->
            - what_needs_cube1.guessed_it and inventory has key:
                You have the key it clearly needs!
                ~ move(key, inventory, first_cube)
            - what_needs_cube1.guessed_it and first_cube has key:
                There's a key above it.
            - else:
                It clearly needs a key.
        } <>
    -   -> top
    
= what_needs_cube1
    Looks like it's missing something.
    *   Maybe it's a rock.
        Thinking about it again, no, it's clearly not a rock.
    *   (guessed_it) Perhaps it's a key.
        Yes, it must be a key!
    *   Probably it's a card.
        But no, it can't be.
    -   ->->



= Cube2
    +   [-- Cube2]
        You stare at the right cube.
        {
            - not what_needs_cube2.guessed_it:
                -> what_needs_cube2 ->
            - what_needs_cube2.guessed_it and inventory has gem:
                It's time to put the gem.
                ~ move(gem, inventory, second_cube)
            - what_needs_cube2.guessed_it and second_cube has gem:
                There's a gem floating above it.
            - else:
                It needs a gem, without doubts.
        } <>
        -   -> top

= what_needs_cube2
    Looks like it needs something.
    - (what_needs_cube2_top)
    *   it surely is an urn.
        Nonetheless, it wouldn't fit.
        -> what_needs_cube2_top
    *   Unquestionably, a feather.
        Which is, sadly, too light.
        -> what_needs_cube2_top
    *   (guessed_it) A gem. Without doubts!
        And indeed it is!
        ->->



= Key
+   {first_cube has key} [-- Key]
    The key levitates over the cube. <>
    -   -> top



= Gem
+   {second_cube has gem} [-- Gem]
    The gem shines over the cube. <>
    -   -> top



= win_check
    { first_cube has key and second_cube has gem:
        ~ win("You solved the very intricate puzzle!")
    }
    -> DONE