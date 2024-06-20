# CosmosGuardian
창작 하이퍼 캐주얼 슈팅 게임 <a href="https://play.google.com/store/apps/details?id=com.Kyungil.CosmosGuardian&pcampaignid=web_share"><코스모스 가디언></a>입니다.

# 개발 기간
2024-05-07 ~ 2024-05-30

# 개발 인원
2인 / 이인호(기획), 김지훈(프로그래머)

# 개발 환경
<img src="https://img.shields.io/badge/Microsoft_Excel-217346?logo=microsoft-excel&logoColor=white"> <img src="https://img.shields.io/badge/Github-000000?style=flat-square&logo=Github&logoColor=ffffff"> <img src="https://img.shields.io/badge/Visual Studio-777777?style=flat-square&logo=Visual Studio&logoColor=purple"> <img src="https://img.shields.io/badge/Unity-ffffff?style=flat-square&logo=Unity&logoColor=000000">

# 협업 도구
<img src="https://img.shields.io/badge/Microsoft_Word-2B509A?logo=microsoft-word&logoColor=white"> <img src="https://img.shields.io/badge/Google Drive-4285F4?logo=googledrive&logoColor=white"> <img src="https://img.shields.io/badge/Discord-7289DA?logo=discord&logoColor=white"> <img src="https://img.shields.io/badge/Notion-white?logo=notion&logoColor=black">

# 프로젝트 소개 
<h3>끊임없는 선택과 성장으로 강해지는 하이퍼 캐주얼 슈팅 게임</h3>

우주 전쟁을 일으킨 외계 생명체를 몰아내기 위해 우주 전쟁에 참가하게 된 20종의 캐릭터와 14종의 무기를 이용해 우주 평화를 위해 싸우는 게임입니다.

# 주요 일정 관리
## 1차 마일스톤  : 2024-05-07 ~ 2024-05-15
- 전투 시스템 개발
  - 플레이어 움직임 및 발사체 전방 발사
  - 플레이어, 몬스터 간 상호작용

- 캐릭터 시스템 개발
  - 캐릭터 테이블을 통한 캐릭터 스텟 적용
  - 선택지를 통해 캐릭터 스텟 업그레이드
  - 무기 시스템과 함께 사용해 플레이어의 스텟을 결정

- 무기 시스템 개발
  - 무기 테이블을 통한 무기 스텟 적용
  - 선택지를 통해 무기 스텟 업그레이드
  - 캐릭터 시스템과 함께 사용해 플레이어의 스텟을 결정

- 선택지 시스템 개발
  - 플레이어 레벨에 알맞은 선택지 목록 추출
  - 중복을 허용하지 않고 선택지 생성
  - 무기 스텟 업그레이드 선택지를 누적
  - 선택지를 통해 무기를 교체할 경우 누적된 선택지를 모두 적용
  - 한 번에 하나의 선택지를 획득, 나머지는 선택지 비활성화

## 2차 마일스톤 : 2024-05-16 ~ 2024-05-23
- 몬스터 시스템 개발
  - 총 5가지 타입의 몬스터 등장
    - 박쥐
      - 플레이어 추격
    - 드래곤
      - 많은 체력
      - 플레이어 미추격
    - 메이지
      - 적은 체력
      - 플레이어 방향으로 발사체 1회 발사
    - 체스트
      - 상당히 많은 체력
      - 피할 수 없는 충돌 범위
      - 처치 시 강화 아이템 획득
    - 웨어울프
      - 상당히 많은 체력
      - 3초마다 플레이어 방향으로 발사체 3회 발사
      - 처치 시 강화 아이템 획득
  - 플랫폼 이동에 따라 몬스터 능력치 증가
  - 몬스터 테이블을 통해 능력치의 가중치 설정

- 아이템(강화) 시스템 개발
  - 중간 보스 또는 보스 처치를 통해 캐릭터 또는 무기 스텟 보상 지급
  - 여러 개의 선택지가가 묶여서 하나의 아이템(강화)을 구성
  - 동일 아이템 반복 선택 시 아이템 레벨 증가

- 음향 효과 적용
  - BGM
  - SFX
    - 무기에 따른 발사체 SFX 변경
  
- 시각 효과 적용
  - 플레이어 오라
    - 기본 오라
    - 선택지 획득 오라
    - 몬스터 스플레시 데미지 오라
  - 발사체
    - 발사 플래시
    - 발사체 충돌
  - 카메라 효과
    - 카메라 쉐이크
      - 몬스터 충돌 시 카메라 쉐이크
    - 시점 이동
      - 보스 몬스터와 전투 진행 시 Top View에서 Tps View로 시점 이동
      - 보스 처치 후 Tps View에서 Top View로 시점 이동

## 3차 마일스톤 : 2024-05-24 ~ 2024-05-30
- 이슈 대응
  - 이슈 1 : 캐릭터와 무기를 선택 후 플레이 했을 때 게임의 단조로움
  - 대응
    - 게임 진행 중 선택지를 통해 무기를 변경하도록 수정

  - 이슈 2 : 투사체 개수와 사거리 증가로 너무 많은 투사체 오브젝트가 생성되고 사라지지 않는 문제
  - 대응
    - 선택지의 사거리 능력치 밸런싱, 제한 설정

  - 이슈 3 : 움직임 없는 몬스터로 인한 게임의 긴장감 저하
  - 대응
    - 플레이어를 추격하는 몬스터 추가
    - 투사체를 발사하는 몬스터 추가

- 최적화
  - 오브젝트 풀링
    - 적용 오브젝트
      - 스테이지 플랫폼, 몬스터, 발사체, 플로팅 텍스트, 시각 효과
  - 컬링
    - 적용 오브젝트
      - 카메라로부터 멀리 떨어져 잘 보이지 않는 플랫폼의 몬스터
