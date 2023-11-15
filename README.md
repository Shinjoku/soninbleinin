# Soninbleinim

An ARPG built in Godot following a [tutorial](https://www.youtube.com/playlist?list=PL9FzW-m48fn2SlrW0KoLT4n5egNdX-W9a) from [Heartbeast](https://github.com/uheartbeast).

It includes a cute feline character that rolls and has a sword and fights enemies.

## Issues

- Bats' soft collision make them fly away too fast sometimes
  - MoveAndCollide has it happening more frequently than MoveAndSlide, but MoveAndSlide makes bats attach to the player
- Taking multiple hits on the same frame results in taking multiple hearts of damage when it should take only 1
