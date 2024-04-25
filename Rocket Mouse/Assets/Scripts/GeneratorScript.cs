using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    public GameObject[] availableRooms; // �߰��� �� ������
    public List<GameObject> currentRooms; // ���� ������ �� ����Ʈ


    float screenWidthInPoints; // ȭ�� ����ũ��(���� : ����)
    const string floor = "Floor";


    public GameObject[] availableObjects;   // �߰��� ������Ʈ ������ ���� �迭
    public List<GameObject> feverCoin;
    public List<GameObject> objects;        // ����, ������ ������Ʈ ����Ʈ
    public float objectsMinDistance = 5f;   // ������Ʈ�� �ּ� ����
    public float objectsMaxDistance = 10f;  // ������Ʈ�� �ִ� ����
    public float objectsMinY = -1.4f;       // ������Ʈ y�� ��ġ �ּ� ��
    public float objectsMaxY = 1.4f;        // ������Ʈ y�� ��ġ �ִ� ��
    public float objectsMinRotation = -45f; // ������Ʈ �ּ� ȸ���� 
    public float objectsMaxRotation = 45f;  // ������Ʈ �ִ� ȸ���� 

    public bool f;

    private void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize; // ī�޶� ������ �� 2��� ���� ���� ������ ���
        screenWidthInPoints = height * Camera.main.aspect; // 
        
    }

    private void Update()
    {
        
        f = GameObject.Find("Mouse").GetComponent<MouseController>().Fever;
        GenerateRoomIrRequired();
        GenerateObjectsIfRequired();
    }

    private void AddRoom(float fartherstRoomEndx)
    {
        int randomRoomIndex = Random.Range(0, availableRooms.Length); // ��� �� �ϳ� ���� ����
        GameObject room = Instantiate(availableRooms[randomRoomIndex]); // ���õ� ���� �߰�
        float roomWidth = room.transform.Find(floor).localScale.x; //���� ���� ũ��
        float roomCenter = fartherstRoomEndx + roomWidth / 2; // ���� �߾� ��ġ
        room.transform.position = new Vector3(roomCenter, 0, 0); // ���� ���� ��ġ�� ������Ʈ ��ġ��Ŵ
        currentRooms.Add(room); // �߰��� ���� ���� �߰��� �渮��Ʈ�� �߰�
    }

    private void GenerateRoomIrRequired()
    {
        
        List<GameObject> roomsToRemove = new List<GameObject>(); //������ �� ��� ���� ����Ʈ
        bool addRooms = true; // ���� �����ӿ� ���� ���� �� ���ΰ�
       
        float playerX = transform.position.x; //���콺 �������� x��
        float removeRoomX = playerX - screenWidthInPoints; //���� �� ���� ���� ��ġ ����
        float addRoomX = playerX + screenWidthInPoints; // �߰� �� ���� ���� ��ġ ����
        float farthestRoomEndX = 0f; // ���� �����ʿ� ��ġ�� ���� �����ʳ� ��ġ

        foreach (var room in currentRooms) // ���� �߰� �� �� �ϳ��� ó��
        {
            float roomWidth = room.transform.Find(floor).localScale.x; // room ������Ʈ�� �ٴڿ�����Ʈ�� ã�� ���� ũ�⸦ ������
            float roomStartX = room.transform.position.x - roomWidth / 2; // ���� �߾���ġ���� ���� ũ�� ������ �� ���� �� ��ġ ��� 
            float roomEndX = roomStartX + roomWidth; // ���� ���� �� ��ġ���� ���� ũ�⸦ ���� ������ �� ��ġ ���
            
            if (roomStartX > addRoomX) // ���� ���� �� ��ġ�� �� �߰� ���� ��ġ���� �����ʿ� �ִٸ� �� �߰� X
                addRooms = false; // 
            
            if (roomEndX < removeRoomX) // �� ���� ���� ��ġ���� ���ʿ� �����ϴ� ���� ������ �� ���� ��Ͽ� �߰� 
                roomsToRemove.Add(room); //

            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX); // ���� ������ ���� ������ �� ��ġ�� �ִ밪 �޼ҵ带 �̿��� ����
        }

        foreach(var room in roomsToRemove) // ������ �� ����� �ϳ��� ����
        {
            currentRooms.Remove(room); // ����Ʈ���� ����
            Destroy(room); // ������Ʈ ����
        }
        if (addRooms) //���� �߰� �ؾ� �Ѵٸ�
            AddRoom(farthestRoomEndX); // �߰�

    }

    private void AddObject(float lastObjectX)
    {
        int randomInedx = Random.Range(0, availableObjects.Length);  // �߰��� ������Ʈ �ε��� �������� ���ϱ�
        GameObject obj = Instantiate(availableObjects[randomInedx]); // ���� ������Ʈ ����
        float objectPositionX =
            lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance); // ���ο� ������Ʈ x�� ��ġ ���
        float randomY = Random.Range(objectsMinY, objectsMaxY);                 // ���ο� ������Ʈ y�� ��ġ ���

        obj.transform.position = new Vector3(objectPositionX, randomY, 0); // ���� ��ġ������ ��ġ ����

        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation); // �������� ȸ���� ���

        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation); // ������Ʈ�� ���� ȸ���� ����
        objects.Add(obj); // ������Ʈ ����Ʈ�� �߰�

    }

    private void FeverTime(float lastObjectX)
    {
        
        
            int randomInedx = Random.Range(0, 3);  // �߰��� ������Ʈ �ε��� �������� ���ϱ�
            GameObject obj = Instantiate(availableObjects[randomInedx]); // ���� ������Ʈ ����
            float objectPositionX =
                lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance); // ���ο� ������Ʈ x�� ��ġ ���
            float randomY = Random.Range(objectsMinY, objectsMaxY);                 // ���ο� ������Ʈ y�� ��ġ ���

            obj.transform.position = new Vector3(objectPositionX, randomY, 0); // ���� ��ġ������ ��ġ ����

            float rotation = Random.Range(objectsMinRotation, objectsMaxRotation); // �������� ȸ���� ���

            obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation); // ������Ʈ�� ���� ȸ���� ����
            feverCoin.Add(obj); // ������Ʈ ����Ʈ�� �߰�
        
    }

    private void GenerateObjectsIfRequired()
    {

        float playerX = transform.position.x;                       // �÷��̾��� x�� ��ġ 
        float removeObjcxtX = playerX - screenWidthInPoints;        // ������ ������Ʈ ���� ��ġ
        float addObjectX = playerX + screenWidthInPoints;           // �߰��� ������Ʈ ���� ��ġ
        float farthestObjectX = 0;                                  // ���� �����ʿ� ��ġ�� ������Ʈ�� x�� ��ġ

        List<GameObject> objectsToRemove = new List<GameObject>();  // ������ ������Ʈ ����Ʈ
        List<GameObject> objectsR = new List<GameObject>();

        if (!f)
        {
            if(feverCoin.Count > 0)
            {
                foreach (var obj in feverCoin) // ���� �߰��Ǿ� �ִ� ������Ʈ�� �ϳ��� ó��
                {
                    float objX = obj.transform.position.x; // ������Ʈ�� x�� ��ġ ��
                    farthestObjectX = Mathf.Max(farthestObjectX, objX);
                    objectsR.Add(obj);
                }
                foreach (var obj in objectsR) // ���� ����Ʈ�� ��ϵǾ� �ִ� �� ��� ����
                {
                    feverCoin.Remove(obj);
                    Destroy(obj);        // ������Ʈ ����
                }
                if (farthestObjectX < addObjectX)
                    FeverTime(farthestObjectX);
            }
            foreach (var obj in objects) // ���� �߰��Ǿ� �ִ� ������Ʈ�� �ϳ��� ó��
            {
                float objX = obj.transform.position.x; // ������Ʈ�� x�� ��ġ ��
                farthestObjectX = Mathf.Max(farthestObjectX, objX); // �ִ밪 �������� ���� �����ʿ� ��ġ�� ������Ʈ ��ġ ����

                if (objX < removeObjcxtX)    // ������Ʈ ��ġ�� ���� ���غ��� �����̸� ������Ʈ ���� ����Ʈ�� �߰�
                    objectsToRemove.Add(obj);
            }

            foreach (var obj in objectsToRemove) // ���� ����Ʈ�� ��ϵǾ� �ִ� �� ��� ����
            {
                objects.Remove(obj); // ����Ʈ���� ����
                Destroy(obj);        // ������Ʈ ����
            }

            if (farthestObjectX < addObjectX) // ���� �����ʿ� ��ġ�� ������Ʈ�� �߰� ��ġ���� �����̸� ���ο� ������Ʈ �߰� 
                AddObject(farthestObjectX);

        }
        else
        {
            if (objects.Count > 0)
            {
                foreach (var obj in objects) // ���� �߰��Ǿ� �ִ� ������Ʈ�� �ϳ��� ó��
                {
                    float objX = obj.transform.position.x; // ������Ʈ�� x�� ��ġ ��
                    farthestObjectX = Mathf.Max(farthestObjectX, objX);
                    objectsToRemove.Add(obj);
                }
                foreach (var obj in objectsToRemove) // ���� ����Ʈ�� ��ϵǾ� �ִ� �� ��� ����
                {
                    objects.Remove(obj);
                    Destroy(obj);        // ������Ʈ ����
                }
                if (farthestObjectX < addObjectX)
                    AddObject(farthestObjectX);
            }

            foreach (var obj in feverCoin) // ���� �߰��Ǿ� �ִ� ������Ʈ�� �ϳ��� ó��
            {
                float objX = obj.transform.position.x; // ������Ʈ�� x�� ��ġ ��
                farthestObjectX = Mathf.Max(farthestObjectX, objX); // �ִ밪 �������� ���� �����ʿ� ��ġ�� ������Ʈ ��ġ ����

                if (objX < removeObjcxtX)    // ������Ʈ ��ġ�� ���� ���غ��� �����̸� ������Ʈ ���� ����Ʈ�� �߰�
                    objectsToRemove.Add(obj);
            }

            foreach (var obj in objectsR) // ���� ����Ʈ�� ��ϵǾ� �ִ� �� ��� ����
            {
                feverCoin.Remove(obj); // ����Ʈ���� ����
                Destroy(obj);        // ������Ʈ ����
            }

            if (farthestObjectX < addObjectX) // ���� �����ʿ� ��ġ�� ������Ʈ�� �߰� ��ġ���� �����̸� ���ο� ������Ʈ �߰� 
                FeverTime(farthestObjectX);
        }
        

    }
    
    
}

