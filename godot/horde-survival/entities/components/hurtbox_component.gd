class_name HurtboxComponent
extends Area2D

@export var max_hp: float = 100
@export var invuln_period: float = 0.5

signal damaged
signal died

var hp: float
var invuln_left: float 

func _ready() -> void:
	hp = max_hp

func _process(delta: float) -> void:
	invuln_left = max(0, invuln_left - delta)

func take_dmg(dmg: float) -> void:
	if invuln_left > 0 or hp <= 0:
		return
	
	hp -= dmg
	invuln_left = invuln_period
	damaged.emit()
	if hp <= 0:
		died.emit()
