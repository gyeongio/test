using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    public GameObject[] availableRooms; // 추가될 방 프리팹
    public List<GameObject> currentRooms; // 현재 생성된 방 리스트


    float screenWidthInPoints; // 화면 가로크기(단위 : 유닛)
    const string floor = "Floor";


    public GameObject[] availableObjects;   // 추가될 오브젝트 프리팹 저장 배열
    public List<GameObject> feverCoin;
    public List<GameObject> objects;        // 코인, 레이저 오브젝트 리스트
    public float objectsMinDistance = 5f;   // 오브제트간 최소 간격
    public float objectsMaxDistance = 10f;  // 오브제트간 최대 간격
    public float objectsMinY = -1.4f;       // 오브젝트 y축 위치 최소 값
    public float objectsMaxY = 1.4f;        // 오브젝트 y축 위치 최대 값
    public float objectsMinRotation = -45f; // 오브젝트 최소 회전값 
    public float objectsMaxRotation = 45f;  // 오브젝트 최대 회전값 

    public bool f;

    private void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize; // 카메라 사이즈 값 2배로 곱해 세로 사이즈 계산
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
        int randomRoomIndex = Random.Range(0, availableRooms.Length); // 방들 중 하나 랜덤 선택
        GameObject room = Instantiate(availableRooms[randomRoomIndex]); // 선택된 방을 추가
        float roomWidth = room.transform.Find(floor).localScale.x; //방의 가로 크기
        float roomCenter = fartherstRoomEndx + roomWidth / 2; // 방의 중앙 위치
        room.transform.position = new Vector3(roomCenter, 0, 0); // 구한 방의 위치로 오브젝트 위치시킴
        currentRooms.Add(room); // 추가한 방을 현재 추가된 방리스트에 추가
    }

    private void GenerateRoomIrRequired()
    {
        
        List<GameObject> roomsToRemove = new List<GameObject>(); //삭제할 방 목록 저장 리스트
        bool addRooms = true; // 지금 프레임에 방을 생성 할 것인가
       
        float playerX = transform.position.x; //마우스 오브젝프 x축
        float removeRoomX = playerX - screenWidthInPoints; //삭제 할 방의 기준 위치 구함
        float addRoomX = playerX + screenWidthInPoints; // 추가 할 방의 기준 위치 구함
        float farthestRoomEndX = 0f; // 가장 오른쪽에 위치한 방의 오른쪽끝 위치

        foreach (var room in currentRooms) // 현재 추가 된 방 하나씩 처리
        {
            float roomWidth = room.transform.Find(floor).localScale.x; // room 오브젝트의 바닥오브젝트를 찾아 가로 크기를 가져옴
            float roomStartX = room.transform.position.x - roomWidth / 2; // 방의 중앙위치에서 방의 크기 절반을 뺀 왼쪽 끝 위치 계산 
            float roomEndX = roomStartX + roomWidth; // 방의 왼쪽 끝 위치에서 방의 크기를 더해 오른쪽 끝 위치 계산
            
            if (roomStartX > addRoomX) // 방의 왼쪽 끝 위치가 방 추가 기준 위치보다 오른쪽에 있다면 방 추가 X
                addRooms = false; // 
            
            if (roomEndX < removeRoomX) // 방 삭제 기준 위치보다 왼쪽에 존재하는 방이 있으면 방 삭제 목록에 추가 
                roomsToRemove.Add(room); //

            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX); // 가장 오른쪽 방의 오른쪽 끝 위치를 최대값 메소드를 이용해 구함
        }

        foreach(var room in roomsToRemove) // 삭제할 방 목록을 하나씩 접근
        {
            currentRooms.Remove(room); // 리스트에서 제거
            Destroy(room); // 오브젝트 제거
        }
        if (addRooms) //방을 추가 해야 한다면
            AddRoom(farthestRoomEndX); // 추가

    }

    private void AddObject(float lastObjectX)
    {
        int randomInedx = Random.Range(0, availableObjects.Length);  // 추가할 오브젝트 인덱스 랜덤으로 구하기
        GameObject obj = Instantiate(availableObjects[randomInedx]); // 구한 오브젝트 생성
        float objectPositionX =
            lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance); // 새로운 오브젝트 x축 위치 계산
        float randomY = Random.Range(objectsMinY, objectsMaxY);                 // 새로운 오브젝트 y축 위치 계산

        obj.transform.position = new Vector3(objectPositionX, randomY, 0); // 계산된 위치값으로 위치 변경

        float rotation = Random.Range(objectsMinRotation, objectsMaxRotation); // 랜덤으로 회전값 계산

        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation); // 오브젝트에 랜덤 회전값 적용
        objects.Add(obj); // 오브젝트 리스트에 추가

    }

    private void FeverTime(float lastObjectX)
    {
        
        
            int randomInedx = Random.Range(0, 3);  // 추가할 오브젝트 인덱스 랜덤으로 구하기
            GameObject obj = Instantiate(availableObjects[randomInedx]); // 구한 오브젝트 생성
            float objectPositionX =
                lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance); // 새로운 오브젝트 x축 위치 계산
            float randomY = Random.Range(objectsMinY, objectsMaxY);                 // 새로운 오브젝트 y축 위치 계산

            obj.transform.position = new Vector3(objectPositionX, randomY, 0); // 계산된 위치값으로 위치 변경

            float rotation = Random.Range(objectsMinRotation, objectsMaxRotation); // 랜덤으로 회전값 계산

            obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation); // 오브젝트에 랜덤 회전값 적용
            feverCoin.Add(obj); // 오브젝트 리스트에 추가
        
    }

    private void GenerateObjectsIfRequired()
    {

        float playerX = transform.position.x;                       // 플레이어의 x축 위치 
        float removeObjcxtX = playerX - screenWidthInPoints;        // 삭제할 오므젝트 기준 위치
        float addObjectX = playerX + screenWidthInPoints;           // 추가할 오브젝트 기준 위치
        float farthestObjectX = 0;                                  // 가장 오른쪽에 위치한 오브젝트의 x축 위치

        List<GameObject> objectsToRemove = new List<GameObject>();  // 삭제할 오브젝트 리스트
        List<GameObject> objectsR = new List<GameObject>();

        if (!f)
        {
            if(feverCoin.Count > 0)
            {
                foreach (var obj in feverCoin) // 현재 추가되어 있는 오브젝트들 하나씩 처리
                {
                    float objX = obj.transform.position.x; // 오브젝트의 x축 위치 값
                    farthestObjectX = Mathf.Max(farthestObjectX, objX);
                    objectsR.Add(obj);
                }
                foreach (var obj in objectsR) // 삭제 리스트에 등록되어 있는 것 모두 제거
                {
                    feverCoin.Remove(obj);
                    Destroy(obj);        // 오브젝트 제거
                }
                if (farthestObjectX < addObjectX)
                    FeverTime(farthestObjectX);
            }
            foreach (var obj in objects) // 현재 추가되어 있는 오브젝트들 하나씩 처리
            {
                float objX = obj.transform.position.x; // 오브젝트의 x축 위치 값
                farthestObjectX = Mathf.Max(farthestObjectX, objX); // 최대값 연산으로 가장 오른쪽에 위치한 오브제트 위치 저장

                if (objX < removeObjcxtX)    // 오브젝트 위치가 삭제 기준보다 왼쪽이면 오브젝트 삭제 리스트에 추가
                    objectsToRemove.Add(obj);
            }

            foreach (var obj in objectsToRemove) // 삭제 리스트에 등록되어 있는 것 모두 제거
            {
                objects.Remove(obj); // 리스트에서 제거
                Destroy(obj);        // 오브젝트 제거
            }

            if (farthestObjectX < addObjectX) // 가장 오른쪽에 위치한 오브제트가 추가 위치보다 왼쪽이면 새로운 오브젝트 추가 
                AddObject(farthestObjectX);

        }
        else
        {
            if (objects.Count > 0)
            {
                foreach (var obj in objects) // 현재 추가되어 있는 오브젝트들 하나씩 처리
                {
                    float objX = obj.transform.position.x; // 오브젝트의 x축 위치 값
                    farthestObjectX = Mathf.Max(farthestObjectX, objX);
                    objectsToRemove.Add(obj);
                }
                foreach (var obj in objectsToRemove) // 삭제 리스트에 등록되어 있는 것 모두 제거
                {
                    objects.Remove(obj);
                    Destroy(obj);        // 오브젝트 제거
                }
                if (farthestObjectX < addObjectX)
                    AddObject(farthestObjectX);
            }

            foreach (var obj in feverCoin) // 현재 추가되어 있는 오브젝트들 하나씩 처리
            {
                float objX = obj.transform.position.x; // 오브젝트의 x축 위치 값
                farthestObjectX = Mathf.Max(farthestObjectX, objX); // 최대값 연산으로 가장 오른쪽에 위치한 오브제트 위치 저장

                if (objX < removeObjcxtX)    // 오브젝트 위치가 삭제 기준보다 왼쪽이면 오브젝트 삭제 리스트에 추가
                    objectsToRemove.Add(obj);
            }

            foreach (var obj in objectsR) // 삭제 리스트에 등록되어 있는 것 모두 제거
            {
                feverCoin.Remove(obj); // 리스트에서 제거
                Destroy(obj);        // 오브젝트 제거
            }

            if (farthestObjectX < addObjectX) // 가장 오른쪽에 위치한 오브제트가 추가 위치보다 왼쪽이면 새로운 오브젝트 추가 
                FeverTime(farthestObjectX);
        }
        

    }
    
    
}

