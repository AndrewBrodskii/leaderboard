Leaderboard — README

Brief description
The project is a simple example of a popup leaderboard window for Unity.
It reads a local JSON file via Addressables, renders a player list, asynchronously loads avatars, and displays a loading indicator on each item.
It’s built with a reusable architecture: View / Controller / Model, ObjectPool, ResourcesManager (Addressables + JSON), and UniTask for async operations.

1. What the project does
	•	Loads a JSON file with leaderboard data (via Addressables / TextAsset).
	•	Builds a scrollable list (ScrollView) with LeaderboardItemView prefabs.
	•	For each player, asynchronously loads an avatar from a URL (using UnityWebRequest), while showing a radial-filled progress spinner during loading.
	•	Supports highlighting/special handling for the player’s own item, including auto-centering within the container.
	•	Uses object pooling (ObjectPool) to minimize Instantiate/Destroy calls.

2. Not implemented
	•	Integration with the suggested PopupManager.
	•	Avatar caching.