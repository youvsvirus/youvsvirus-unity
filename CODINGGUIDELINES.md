# Unity Coding and Development Guidelines

We use Unity Version 2019.3.7, as available from the [Unity Hub](https://unity3d.com/de/get-unity/download). Due to incompatibility between versions, all contributors have to use this version.

## Git Workflow

1. Pick an unassigned issue from the GitHub repository and assign it to yourself.
2. Create a branch `yourname>/<yourissue>` for work on that issue.
3. Implement and *test* your code.
4. Merge the (potentially changed) master branch into your issue branch
5. Push your issue branch and create a pull request.

**Always commit your `.meta` files! Otherwise, the project breaks!**

For details on resolving merge conflicts in Unity Files, see [Unity Merging](MERGING.md).

## Folder Structure

To keep our project in order, all files should be stored in their dedicated folder inside the *Assets* directory, depending on content type. Inside each content type folder, further structure should be achieved by creating subfolders, e.g. *Sprites/SmileyPictures* for all smiley images.

- **Audio** : Folder for audio files
- **Materials** : Folder for Unity Materials
- **Prefabs** : All prefabs go in here.
- **Scenes** : All Unity Scenes belong in here
- **Sprites** : Folder for all sprite textures
- **Scripts** : All C#-Scripts belong in this folder and its subfolders.

## C# Scripting Guidelines

- Script files belong in the *Assets/Scripts* folder and its subfolders.
- Each Code file should contain *exactly one class*; file and class name should be the same. E.g. *Player.cs* contains only the `Player` class.
- According to C# style guidelines:
    - All *class names* should be Upper Camel Case (e.g. `Player`, `GameState`)
    - All *method names* should be Upper Camel Case (e.g. `Update()`, `SetAnimationState()`, ...)
    - All *private* member variables should be lower Camel Case
- Namespaces:
    - Classes contained directly in the *Scripts* folder should be inside the default namespace (no `namespace` environment!)
    - Classes contained in a subfolder of the *Scripts* folder should be contained in a namespace reflecting this folder's name, e.g. for `Player.cs` in the *Components* subfolder:
        ```
            namespace Components {
                public class Player : MonoBehaviour {
                    (...)
                }
            }
        ```
    - All classes derived from `MonoBehaviour` are components which can be attached to GameObjects in the Editor. They should be contained in the *Scripts/Components* folder and, thus, in the `Components` namespace.
- Comments and Documentation:
    - Each public method needs to have a short documentation comment, describing its semantics. These should be written in C# doc comment syntax:
        ```
                /// <summary>
                ///     This method calculates something.
                /// </summary>
                /// <param name="input">An input parameters</param>
                /// <returns>The result of the calculation.</returns>
                public int calculateSomething(double input) { (...) }
        ```
        When using Microsoft Visual Studio as a code editor, simply type three slashes (`///`) above the method declaration and the IDE will automatically create an empty stub for the doc comment.