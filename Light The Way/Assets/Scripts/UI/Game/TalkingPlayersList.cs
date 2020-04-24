using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingPlayersList : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> talkingPlayerTransformList;

    public float templateHeight = 20f;

    // Start is called before the first frame update
    void Awake()
    {
        entryContainer = transform.Find("TalkingPlayersContainer");
        entryTemplate = entryContainer.Find("TalkingPlayerTemplate");

        entryTemplate.gameObject.SetActive(false);

        talkingPlayerTransformList = new List<Transform>();
    }

    void Update()
    {
        foreach(Transform transform in talkingPlayerTransformList)
        {
            transform.gameObject.SetActive(false);
            Destroy(transform.gameObject);
        }
        talkingPlayerTransformList.Clear();

        foreach (KeyValuePair<string, bool> entry in MasterManager.TalkingPlayers)
        {
            if(entry.Value)
                CreateTalkingPlayerTransform(entry.Key, entryContainer, talkingPlayerTransformList);
        }
    }

    private void CreateTalkingPlayerTransform(string playerName, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("PlayerName").GetComponent<Text>().text = playerName;
        transformList.Add(entryTransform);
    }
}
