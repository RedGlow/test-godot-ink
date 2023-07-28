VAR IN_GODOT = false



// OBJECTS AND INVENTORY MANAGEMENT

LIST Objects = key, gem
VAR second_room = (key, gem)
VAR inventory = ()
VAR first_cube = ()
VAR second_cube = ()

=== function add(object, ref to)
    ~ to += object
    
=== function move(object, ref from, ref to)
    ~ from -= object
    ~ to += object



// ROOM MOVEMENT

=== goTo(-> divert)
    ~ goToRoom(divert)
    -> divert

EXTERNAL goToRoom(divert)
=== function goToRoom(divert)
    ~ temp a = 0



// WIN

EXTERNAL win()
=== function win()
    YOU WON