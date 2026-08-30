class_name HitboxComponent
extends Area2D

@export var dmg: float = 20

func _process(delta: float) -> void:
	var areas: Array[Area2D] = get_overlapping_areas()
	for area in areas:
		if area is not HurtboxComponent: continue
		var hurtbox: HurtboxComponent = area as HurtboxComponent
		hurtbox.take_dmg(dmg)
