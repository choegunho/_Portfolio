# [Unity 3D] TopDown Roguelike Game Portfolio
# 1. 소개
<img width="400" height="480" alt="image" src="https://github.com/user-attachments/assets/7b773bf1-6f98-42c0-93de-9360e9059608" /> <img width="400" height="480" alt="image" src="https://github.com/user-attachments/assets/498f8838-7699-43a9-b809-61f88dad3ae8" /> 
<img width="400" height="480" alt="image" src="https://github.com/user-attachments/assets/c4ef4cbb-4aa1-4930-8945-d3c559071433" /> <img width="400" height="480" alt="image" src="https://github.com/user-attachments/assets/d8a5af3c-d941-49de-a53a-0f1212d6a2a3" />

- Unity 3D Roguelike 게임입니다
- 개발기간: 2025.12.08 ~ 2026.01.30 (약 2개월)
- 플레이어가 몬스터와 전투하며 레벨업을 진행하는 3D 액션 게임입니다.

---

# 2. 개발 환경
- Unity 6000.0.54f1 LTS
- C#
- Window 11

--- 

# 3. 사용 기술
- Unity (C#)
- FSM (State Pattern)
- NavMesh AI
- ScriptableObject
- Async Scene Loading
- Trigger / Overlap 기반 충돌 처리

---

# 4. 주요 기능

### 🔹 Player System
- WASD 이동 & 마우스 방향 회전
- FSM 기반 상태 관리 (Idle / Move / Attack / Defend / Dead)
- 근접 공격 + 스킬 공격 시스템

### 🔹 Monster AI System
- FSM 기반 상태 (Idle / Chase / Attack / Dead)
- NavMesh를 이용한 플레이어 추적
- EXP & LevelUp
- Overlap 기반 공격 판정

### 🔹 Combat System
- Player: Trigger 기반 공격 판정
- Monster: Overlap 기반 공격 판정
- 데미지 계산 및 상태 반영

### 🔹 UI System
- HP / EXP UI
- MiniMap UI
- Level Up UI

### 🔹 Scene System
- Persistent Scene + World Scene 구조
- Async Scene Loading을 통한 자연스러운 씬 전환

---

## 기술적 특징

### ✅ FSM 상태 관리
- Monster의 FSM방식은 Switch-Case 기반으로 몬스터의 현재 상태에 따라 상태가 변하도록 설계했습니다.
- Player FSM방식은 Switch-Case 기반이 아닌 State 클래스를 분리하여 유지보수성과 확장성을 고려한 구조로 설계했습니다.

---

### ✅ 공격 판정 최적화
- Player: Collider Trigger 방식 사용
- Monster: Overlap 방식 사용

---

### ✅ 데이터 관리
ScriptableObject를 활용하여  
데이터와 로직을 분리하고 재사용성을 높였습니다.

---

## Player System

### FSM(Finite State Machine)

플레이어의 상태는 Idle / Move / Attack / Defense / Skill / Dead 상태가 있습니다.

- Idle

  - 움직임이 감지되면 Move / Attack / Defense / Skill 상태로 전환
 
- Move

  - WASD 키로 이동
  - 아무런키를 누르지 않으면 Idle 상태로 전환
 
- Attack

  - 마우스 좌클릭으로 공격
  - 공격 애니메이션 재생 및 종료시 입력 감지
  - 입력감지 없을 시 Idle 상태로 전환
 
- Defense

  - 마우스 우클릭 유지로 방어
  - 마우스 우클릭을 떼는 순간 방어 애니메이션 종료 및 입력 감지
  - 입력 감지 없을 시 Idle 상태로 전환
 
- Skill

  - Q, R 키로 스킬 사용
  - 스킬 애니메이션 재생 및 종료시 입력 감지
  - 입력 감지 없을 시 Idle 상태로 전환
 
- Dead

  - Dead 애니메이션 재생 및 종료 시 게임 종료
 
---

## Monster AI System

### FSM(Finite State Machine)

몬스터 상태는 Wander / Chase / Attack / Hit / Dead 상태가 있습니다.

- Wander

  - 랜덤한 위치로 배회
  - 타겟을 감지하면 Chase 상태로 전환
 
- Chase

  - NavMesh를 통해 타겟 추적
  - 공격 범위 안으로 들어오면 Attack 상태로 전환
  - 타겟이 멀어지면 Wander 상태로 전환
 
- Attack

  - 공격 애니메이션 재생
  - 거리가 멀어지면 Chase 및 Wander 상태로 전환
 
- Hit

  - 피격 애니메이션 재생
  - 타겟과의 거리에 따라 Wander / Chase / Attack 상태로 전환
 
- Dead

  - 사망 애니메이션 재생 및 제거
 
---

## Combat System

### Attack Decision

플레이어와 몬스터의 공격 판정은 서로 다르게 설정했습니다.

- 플레이어는 무기의 Collider Trigger를 통하여 몬스터에게 데미지를 가하게 하였습니다.

- 몬스터는 각각의 몬스터의 피격 지점을 Collider를 설정하는 것 보다는 범용성을 위하여 Overlap 방식을 사용했습니다.

### EXP & LevelUp

플레이어는 몬스터와의 전투를 통하여 경험치를 얻고 레벨업을 할 수 있습니다.

레벨업을 하면 기본 스탯이 오르고, 추가 능력을 선택하여 강해질 수 있습니다.

각 능력은 랜덤한 3개가 등장하고 한개만 고를 수 있습니다.

<img width="500" height="580" alt="스크린샷 2026-03-19 030840" src="https://github.com/user-attachments/assets/039be3f8-4d01-443c-908c-8bc766897c1a" />

각각의 능력은 ScriptableObject로 데이터화 하여 관리 하였습니다.

---

## UI System

### MiniMap UI

미니맵을 통하여 플레이어가 현재 위치하고 있는 정보를 확인 할 수 있습니다.

- 미니맵은 전역에서 관리하기 위해 Singleton패턴을 사용했습니다.

- 월드 좌표를 그리드 단위로 정규화 하여 미니맵 상에서 방의 위치를 효율적으로 관리하도록 구현했습니다.

- 플레이어가 현재 위치하는 방을 표현하기 위해서 UI 색상을 변경하는 방식으로 상태를 표현했습니다.

<img width="124" height="116" alt="스크린샷 2026-03-18 010002" src="https://github.com/user-attachments/assets/e42cf9af-a421-4a82-a113-b6db36e18fa6" />
<img width="124" height="116" alt="스크린샷 2026-03-18 010058" src="https://github.com/user-attachments/assets/4f8816c4-d2f2-4080-b509-93ba2b6b2146" />

### Room Controller

미니맵에서 보이는 방은 Room Controller를 통하여 문 이벤트 및 몬스터 스폰이 이루어집니다.

- 플레이어가 Room에 입장하는 순간 Trigger를 통하여 모든 문이 닫힐 수 있게 했습니다.

- 문이 닫히고 몬스터가 생성되며, 모든 몬스터 처치 시 문이 열리게 됩니다.

### Wall Transparency

방안에 있는 벽들은 카메라와 플레이어 사이에 들어오게 되면 투명화 처리가 되게 했습니다.

<img width="400" height="480" alt="스크린샷 2026-03-19 035431" src="https://github.com/user-attachments/assets/fbda2783-ff1d-4757-91e6-8cc60728e914" />
<img width="400 height="480" alt="스크린샷 2026-03-19 035450" src="https://github.com/user-attachments/assets/98bf2f37-5a81-4dfa-b5f9-aaf2295c08af" />

---

## Scene System

### Async Scene Loading

Async Scene Loading을 사용하여 씬 전환 시 로딩 지연 없이 자연스럽게 전환되도록 구현했습니다.

Persistent Scene + Stage Scene 분리 구조를 사용하여 UI, 카메라, 매니저 객체를 유지하여 스테이지만 교체하는 방법을 사용했습니다.

<img width="700" height="1024" alt="ChatGPT Image 2026년 3월 18일 오전 02_17_10" src="https://github.com/user-attachments/assets/5c81796c-c5ab-4ce3-be40-a9f647b9205e" />

---

## 5. 시연 영상

https://youtu.be/jWga1h7cnaI

---

## 6. 기술서

https://drive.google.com/file/d/1ErhBrReeaxaE8WXuUXdnkKSfICf0rqGa/view?usp=drive_link
