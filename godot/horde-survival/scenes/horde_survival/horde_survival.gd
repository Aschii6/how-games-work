extends Node2D

const GOBLIN = preload("uid://cabb5wrdti33")

@onready var player: Player = $Player
@onready var wave_timer: Timer = $WaveTimer
@onready var label: Label = $Control/MarginContainer/Label

var wave: int = 1
var goblins: Array[Goblin] = []

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	wave_timer.start(5)
	wave_timer.timeout.connect(_start_wave)
	player.hp_changed.connect(_on_player_hp_changed)

func _start_wave() -> void:
	var min_g: int = int(wave * 1.5) + 1
	var max_g: int = wave + 2
	
	var count = randi_range(min_g, max_g)
	for i in range(count):
		var goblin: Goblin = GOBLIN.instantiate()
		goblin.position = Vector2(randi_range(0, 1280), randi_range(720, 1200))
		goblin.player = player
		goblin.died.connect(_on_goblin_died)
		goblins.push_back(goblin)
		add_child(goblin)

func _on_player_hp_changed(new_hp: float) -> void:
	label.text = "HP: " + var_to_str(int(new_hp))
	if (new_hp <= 0):
		get_tree().quit() # Game Over - Exit

func _on_goblin_died(goblin: Goblin):
	goblins.erase(goblin)
	
	if goblins.is_empty():
		wave_timer.start(15)
