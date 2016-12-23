using UnityEngine;
using System.Collections;

public enum GameState {INACTIVE, ACTIVE};

public class Constants : MonoBehaviour
{

	public static string OBJ_FIRE_HYDRANT = "Fire Hydrant";
	public static string OBJ_WATER_TANK = "Water Tank";
	public static string OBJ_WATER_SEGMENT = "Water Segment";
	public static string OBJ_SMALL_DOG = "Small Dog";
	public static string OBJ_DOG_SPAWNER = "Dog Spawner";
	public static string OBJ_WATER_PUMPER = "Water Pumper";
	public static string DOG_TAG = "Dog";

	public static string OBJ_WATER_SPLASH = "Water Animation";
	public static string ANIM_WATER_SPLASH = "WaterSplash";
	public static string BOOL_WATER_SPLASH = "toSplash";

	public static string INT_DOG_STATE = "State";

	public static string BOOL_PEE_RIGHT = "ShouldPeeRight";
	public static string BOOL_PEE_LEFT = "ShouldPeeLeft";

	public static string ANIM_PEE_RIGHT = "Pee Left";
	public static string ANIM_PEE_LEFT = "Pee Right";

	public static string UI_MORALE_BAR = "Morale Bar";
	public static string UI_MORALE_CANVAS = "Morale Canvas";

	public static string TEXT_GAME_TITLE = "Title";
	public static string UI_START_MENU = "Start Menu";
	public static string UI_GAME_MENU = "Game Menu";
	public static string UI_GAME_OVER_MENU = "Game Over Menu";
	public static string TEXT_PLAY_TITLE = "Play";
	public static string TEXT_RESUME = "Resume";
	public static string TEXT_RESTART = "Restart";
	public static string TEXT_SCORE = "Count";

	public static string GOBJ_GAME_MANAGER = "Game Manager";
	public static string TAG_HYDRANTS = "Hydrants";

	public static string TAG_POWER_UP = "PowerUps";

	public static string POWER_UP_HEALTH = "Health Power Up";
	public static string POWER_UP_SPEED = "Speed Power Up";
	public static string POWER_UP_FREQ = "Frequency Power Up";

	public static string GOBJ_POWER_UP_SPAWNER = "PowerUp Spawner";

}

