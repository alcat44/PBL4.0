using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public List<GameObject> instantiatedItems = new List<GameObject>();

    public Transform ItemContent;
    public GameObject InventoryItem, Inventory, Info, pickupText, dropText;
    public GameObject MapCanvas;
    
    //public GameObject Serundeng, Telor, BerasKetan, Lantern, Kertas, KerakTelor;
    public GameObject itemInstance;
    public Transform player;
    public Vector3 itemPlacementPosition;
    public TMP_Text objective;
    public Image MapImage;
    public Button NextMapButton, PreviousMapButton;
    public bool map;
    public bool isInventoryOpen = false;
    public bool equip = false;
    public bool Reward = false;
    public int Index, Id, currentMapIndex;

    private void Awake()
    {
        Instance = this;
        Inventory.SetActive(false);
    }
    

    public void Add(Item item)
    {
        Items.Add(item);
        ListItems();
        if (item.id == 1 || item.id == 2 || item.id == 6 || item.id == 7|| item.id == 8|| item.id == 9 || item.id == 10 || item.id == 11 || item.id == 12)
        {
            UpdateObjective(item.id);
        }
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        ListItems();
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemNo = obj.transform.Find("ItemNo").GetComponent<TextMeshProUGUI>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemNo.text = Items.IndexOf(item).ToString();
        }
    }

    private void UseMap()
    {
        // Check if player has item with ID 1
        Item mapItem = Items.Find(item => item.id == 1);
        if (mapItem != null)
        {
            map = !map;
            // Activate the map canvas and show the first map image
            MapCanvas.gameObject.SetActive(map);
            currentMapIndex = 0;
            MapImage.sprite = mapItem.images[currentMapIndex]; // Assuming 'images' is a list of Sprites in the Item scriptable object

            // Assign button functions to switch maps
            NextMapButton.onClick.AddListener(() => ShowNextMap(mapItem));
            PreviousMapButton.onClick.AddListener(() => ShowPreviousMap(mapItem));
            if(map == false)
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else{
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            Debug.LogWarning("Player does not have the map item.");
        }
    }

    private void ShowNextMap(Item item)
    {
        if (item.images.Count > 0)
        {
            currentMapIndex = (currentMapIndex + 1) % item.images.Count;
            MapImage.sprite = item.images[currentMapIndex];
        }
        else
        {
            Debug.LogWarning("No maps available in the item.");
        }
    }

    private void ShowPreviousMap(Item item)
    {
        if (item.images.Count > 0)
        {
            currentMapIndex = (currentMapIndex - 1 + item.images.Count) % item.images.Count;
            MapImage.sprite = item.images[currentMapIndex];
        }
        else
        {
            Debug.LogWarning("No maps available in the item.");
        }
    }

    public void UpdateObjective(int itemId)
    {
        Item Beras = Items.Find(item => item.id == 7);
        Item kelapa = Items.Find(item => item.id == 8);
        Item Telur = Items.Find(item => item.id == 9);
        if(Beras != null && kelapa != null && Telur != null)
        {
            objective.text = "Put the Ingredients on the wajan in the 8 Icon gallery 1st floor!";
        }
        switch (itemId)
        {
            case 1:
                objective.text = "Press m to open the map.\n Tour the 1st floor";
                break;
            case 2:
                objective.text = "Match the clue number with the banner number in the third floor!\n And go to the gambang kromong gallery";
                break;
            case 6:
                objective.text = "Check the photo with the food in the traditional food gallery second floor!";
                break;
            case 10:
                objective.text = "Put the kerak telor on the plate in the traditional food gallery second floor!";
                break;
            case 11:
            objective.text = "Take the lantern on the table! \nMatch the clue number with the banner number in the third floor!\n And go to the gambang kromong gallery";
                break;
                case 12:
            objective.text = "Put the Mini Ondel-ondel on the table on the table traditional house in the 8 icon gallery!";
                break;
            default:
                break;
        }
    }

    public void UpdateItemInformation(Item item)
    {
        if (Info != null)
        {
            var itemInformation = Info.transform.Find("ItemInformation").GetComponent<TextMeshProUGUI>();
            var information = Info.transform.Find("Information").GetComponent<TextMeshProUGUI>();

            if (itemInformation != null)
            {
                itemInformation.text = item.itemName;
            }
            else
            {
                Debug.LogError("ItemInformation TextMeshProUGUI component not found in Info.");
            }

            if (information != null)
            {
                information.text = item.itemInformation;
            }
            else
            {
                Debug.LogError("Information TextMeshProUGUI component not found in Info.");
            }
        }
        else
        {
            Debug.LogError("Info GameObject is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        //Id = InventoryManager.Instance.Items[Index].id;
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            UseMap();
        }

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseItem(i);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Unequip();
            }
        }

        
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        Inventory.SetActive(isInventoryOpen);
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Items.Count)
        {
            Debug.LogError("Invalid inventory slot index.");
            return;
        }

        var item = Items[slotIndex];
        Index = slotIndex;
        Id = InventoryManager.Instance.Items[Index].id;
        if (item != null)
        {
            Debug.Log($"Using item: {item.itemName}");

            foreach (var instantiatedItem in instantiatedItems)
            {
                if (instantiatedItem != null)
                {
                    Destroy(instantiatedItem);
                }
            }
            instantiatedItems.Clear();

            if (item.prefab != null && player != null)
            {
                itemInstance = Instantiate(item.prefab, player.position, Quaternion.Euler(0, 0, 0));
                itemInstance.transform.SetParent(player);
                

                instantiatedItems.Add(itemInstance);

                ItemPickUp itemPickUp = itemInstance.GetComponent<ItemPickUp>();
                //ItemDrop itemDrop = itemInstance.GetComponent<ItemDrop>();

                itemPickUp.interactable = false;
                //itemDrop.interactable = false;
                if (itemPickUp != null)
                {
                    //itemPickUp.enabled = false;
                    //itemDrop.enabled = true;
                    //itemPickUp.inventoryManager = this;
                    itemPickUp.pickupText = pickupText;
                    //itemDrop.dropText = dropText;
                    //itemDrop.dropText.SetActive(false);
                    itemPickUp.pickupText.SetActive(false);
                }
                else
                {
                    Debug.LogError("ItemPickUp script not found on item instance.");
                }

                //if (itemDrop != null)
                //{
                    //itemDrop.inventoryManager = this;
                    //itemDrop.dropText = dropText;
                    
                    //itemDrop.dropText.SetActive(false);
                //}
               // else
                //{
                    //Debug.LogError("ItemDrop script not found on item instance.");
                //}

                equip = true;
                Info.SetActive(true);
            }
            else
            {
                Debug.LogError("Item prefab or spawn point not set.");
            }

            UpdateItemInformation(item);
        }
    }

    private void Unequip()
    {
        foreach (var instantiatedItem in instantiatedItems)
        {
            if (instantiatedItem != null)
            {
                Destroy(instantiatedItem);
            }
        }
        Id = 0;
        Index = 0;
        Info.SetActive(false);
        instantiatedItems.Clear();
        equip = false;
    }

    public void PutBaju(Vector3 position)
    {
        if (instantiatedItems.Count > 0)
        {
            GameObject itemToPlace = instantiatedItems[0];
            itemToPlace.transform.SetParent(null);
            itemToPlace.transform.position = position;

            instantiatedItems.RemoveAt(0);
            instantiatedItems.Add(itemToPlace); // Add the placed item back to the list
        }
        else
        {
            Debug.LogError("No item to place.");
        }
    }

    
}
