# High Performance Course - ITU 2024
we trying

## Dev-Log
### 30 - October
We imported the first iteration of packages and planned some individual research for the next meeting

### 12th November
Added a folder environment for test scenes. This includes some tutorial works and project work.
For the project - Added a test scene for spawning in archers in Battalion formation. Slight issues with model scaling.

### 27th November

We need an environment for the archers to fight in. We talked about doing a Castle made up of primitives and a slide for the boulder to go down on.
Add constraints to archers in order for them to not fall over, then possibly disable them when Collision Event is triggered by lethal object collision.
Projectile flying physics
Tagging archers with a IsDead tag (or disable a IsAlive tag) when they are hit by lethal objects.


# Optimization steps:
Deploy divide and conquer search algorithm for closest target.
Other profiling stuff
