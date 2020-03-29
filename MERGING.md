# How to resolve merge conflicts in unity assets

The Best Way to resolve merge conflicts is to **not** create them. But sometimes, they become inevitable or just happen by accident.

Unity stores its assets and scenes in the YAML format. When merge conflicts occur in these files, regular merge tools might not be able to resolve merge conflicts in a semantically correct way. 

## Prequesites

All you need (in addition to Git and Unity) is a visual merge tool like Meld. Also, in the `.gitattributes` file of our repository, UnityYAMLMerge should be specified as the merge tool for use with Unity Asset files:

```
*.meta -text merge=unityyamlmerge diff
*.unity -text merge=unityyamlmerge diff
*.asset -text merge=unityyamlmerge diff
*.prefab -text merge=unityyamlmerge diff
```

## Setup

For merging Unity Assets, you need to set up a few things:

### 1. Add the Unity Smart Merge tool to your PATH

Find the installation directory of your Unity Editor (e.g. `C:\Program Files\Unity\Hub\Editor\2019.3.7f1`) and there, navigate to `Editor\Data\Tools\`. Add this folder (`C:\Program Files\Unity\Hub\Editor\2019.3.7f1\Editor\Data\Tools`) to your PATH environment variable.

### 2. Configure Unity Smart Merge as a merge tool for git

Navigate to 'C:\Users\<your-username>', open '.gitconfig' and add these lines:

```
[merge]
	tool = unityyamlmerge

[mergetool "unityyamlmerge"]
	trustExitCode = false
	cmd = UnityYAMLMerge merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```

### 3. Configure UnityYAMLMerge to open Meld as a visual merge tool

In your Unity Editor's Tools folder, which you located in step 1, open `mergespecfile.txt`. Replace these two lines:

```
unity use "%programs%\YouFallbackMergeToolForScenesHere.exe" "%l" "%r" "%b" "%d"
prefab use "%programs%\YouFallbackMergeToolForPrefabsHere.exe" "%l" "%r" "%b" "%d"
```

by 

```
* use "<meld-installation-path>\meld.exe" "%b" "%r" "%l" -o "%d"
```

containing the path to your installation of Meld.


## Resolving merge conflicts

Start merging by executing `git merge <branch-to-merge-from>` in your command line. If there are any, Git will tell you about merge conflicts in Unity files and ask you to resolve them. To do so, run `git mergetool`. This will run UnityYAMLMerge, which will try to automatically resolve the merge conflicts. If it can't, it in turn will run your visual merge tool (e.g. Meld), where you can resolve the merge conflicts. When you are done, save your changes and close your visual merge tool. Before commiting your conflict resolution, open the Unity Editor and check if everything looks fine.