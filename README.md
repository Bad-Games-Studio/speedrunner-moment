# Speedrunner Moment

This demo is intended to show how animations in Unity work (or how they don't :trollface:).

It does not use root motion, but uses custom movement script instead. It imposes a number of different challenges, but I prefer doing it this way.

Note: the goal was making smooth animations, so it was decided to scrap `Input.GetAxis` because it changes value to `0` when a player wants to quickly change direction along one axis. ~~(Keyboard gaming best)~~. There of course might be an alternative solution that uses this thing.

## Controls

- `WASD` or `Arrows` for movement.
- Hold `Right Mouse Button` and move `Mouse` to rotate the camera and the character.

![speedrunner_moment](https://user-images.githubusercontent.com/49134679/162587897-3016999b-8e6f-4ee8-b1f5-983efe1826b9.png)

## Credits

- [Mixamo](https://www.mixamo.com) for motion animations, and a character model.
- [iHeartGameDev](https://www.youtube.com/c/iHeartGameDev) for a [playlist](https://www.youtube.com/playlist?list=PLwyUzJb_FNeTQwyGujWRLqnfKpV-cj-eO) with great tutorials.
- [Google](https://www.google.com/), [Stack Overflow](https://stackoverflow.com/) and [Unity docs](https://docs.unity3d.com/ScriptReference/) for being here when no one else was...
