// OBJECTS AND INVENTORY MANAGEMENT

LIST Objects = key, gem
VAR second_room = (gem, key)
VAR inventory = ()
VAR first_cube = ()
VAR second_cube = ()

=== function add(object, ref to)
    ~ to += object
    
=== function move(object, ref from, ref to)
    ~ from -= object
    ~ to += object



// ROOM MOVEMENT

=== go_to(-> divert)
    ~ go_to_room(divert)
    -> divert

EXTERNAL go_to_room(divert)
=== function go_to_room(divert)
    ~ temp a = 0



// WIN

EXTERNAL win(text)
=== function win(text)
    {text}
    [the game will end here]