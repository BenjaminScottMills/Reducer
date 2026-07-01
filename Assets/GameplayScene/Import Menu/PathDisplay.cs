using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PathDisplay : MonoBehaviour
{
    public ImportFolderContents importFolderContents;
    public RectTransform rectTransform;
    public GameObject folderButtonPrefab;
    public List<PathFolderButton> folderButtons;

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateFolderButtons(ImportMenu.DirectoryLevel currLevel, string currPath, RFolder currentFolder)
    {
        folderButtons.ForEach(b => Destroy(b.gameObject));
        folderButtons.Clear();
        List<(ImportMenu.DirectoryLevel level, RFolder folder)> buttonInfos = new();
        string solutionPath;
        
        switch (currLevel)
        {
            case ImportMenu.DirectoryLevel.chapters:
                buttonInfos.Add((ImportMenu.DirectoryLevel.chapters, null));
                solutionPath = Path.Combine(currPath, "dummyChapter", "dummyLevel", "solutions", "dummySolution");
                break;
            case ImportMenu.DirectoryLevel.levels:
                buttonInfos.Add((ImportMenu.DirectoryLevel.chapters, null));
                buttonInfos.Add((ImportMenu.DirectoryLevel.levels, null));
                solutionPath = Path.Combine(currPath, "dummyLevel", "solutions", "dummySolution");
                break;
            case ImportMenu.DirectoryLevel.solutions:
                buttonInfos.Add((ImportMenu.DirectoryLevel.chapters, null));
                buttonInfos.Add((ImportMenu.DirectoryLevel.levels, null));
                buttonInfos.Add((ImportMenu.DirectoryLevel.solutions, null));
                solutionPath = Path.Combine(currPath, "dummySolution");
                break;
            case ImportMenu.DirectoryLevel.specificSolution:
                solutionPath = currPath;
                List<(ImportMenu.DirectoryLevel, RFolder)> reversedFolders = new();
                while (currentFolder != null)
                {
                    buttonInfos.Add((ImportMenu.DirectoryLevel.specificSolution, currentFolder));
                    currentFolder = currentFolder.parentFolder;
                }
                buttonInfos.Add((ImportMenu.DirectoryLevel.specificSolution, currentFolder));
                buttonInfos.Add((ImportMenu.DirectoryLevel.solutions, null));
                buttonInfos.Add((ImportMenu.DirectoryLevel.levels, null));
                buttonInfos.Add((ImportMenu.DirectoryLevel.chapters, null));
                buttonInfos.Reverse();
                break;
            default:
                throw new Exception("case fallthrough error in PathDisplay.cs");
        }

        Vector3 nextPosition = TopLeftCorner(rectTransform);
        foreach (var pair in buttonInfos)
        {
            PathFolderButton newButton = Instantiate(folderButtonPrefab, nextPosition, Quaternion.identity, transform).GetComponent<PathFolderButton>();
            folderButtons.Add(newButton);
            newButton.Setup(pair.level, pair.folder, solutionPath, importFolderContents);
            nextPosition = TopRightCorner(newButton.rectTransform);
        }

        float contentRightEdge = TopRightCorner(folderButtons.Last().rectTransform).x;
        float boxRightEdge = TopRightCorner(rectTransform).x;
        if (contentRightEdge > boxRightEdge)
        {
            Vector3 requiredDisplacement = new Vector3(boxRightEdge - contentRightEdge, 0);
            folderButtons.ForEach(b => b.transform.position += requiredDisplacement);
        }
    }

    public void CreateFolderButtonsFavourites()
    {
        folderButtons.ForEach(b => Destroy(b.gameObject));
        folderButtons.Clear();
        PathFolderButton newButton = Instantiate(folderButtonPrefab, TopLeftCorner(rectTransform), Quaternion.identity, transform).GetComponent<PathFolderButton>();
        folderButtons.Add(newButton);
        newButton.Setup(ImportMenu.DirectoryLevel.chapters, null, "", importFolderContents);
        newButton = Instantiate(folderButtonPrefab, TopRightCorner(newButton.rectTransform), Quaternion.identity, transform).GetComponent<PathFolderButton>();
        folderButtons.Add(newButton);
        newButton.SetupFavourites();
    }

    static Vector3 TopRightCorner(RectTransform rt)
    {
        Vector3[] cornersArray = new Vector3[4];
        rt.GetWorldCorners(cornersArray);
        return cornersArray[2];
    }

    static Vector3 TopLeftCorner(RectTransform rt)
    {
        Vector3[] cornersArray = new Vector3[4];
        rt.GetWorldCorners(cornersArray);
        return cornersArray[1];
    }
}
