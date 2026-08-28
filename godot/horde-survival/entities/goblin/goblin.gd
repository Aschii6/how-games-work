extends Node2D

@export var player: Player

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

const SPEED: float = 200.0
const ACCELERATION: float = SPEED * 4;

enum State {IDLE, RUN, ATTACK}
var state: State = State.IDLE

var velocity: Vector2 = Vector2.ZERO

var attack_cooldown_remaining: float = 0.0

func _ready() -> void:
	animated_sprite_2d.animation_finished.connect(_on_animation_finished)

func _process(delta: float) -> void:
	attack_cooldown_remaining = max(0.0, attack_cooldown_remaining - delta)
	if state == State.ATTACK: return
	
	var distance: float = global_position.distance_to(player.global_position)
	var direction: Vector2 = global_position.direction_to(player.global_position)
	var target_velocity: Vector2
	
	if distance < 64:
		target_velocity = Vector2.ZERO
		_set_state(State.ATTACK if attack_cooldown_remaining == 0.0 else State.IDLE)
	else:
		target_velocity = direction * SPEED
		_set_state(State.RUN)
	
	velocity = velocity.move_toward(target_velocity, ACCELERATION * delta)
	if !is_zero_approx(direction.x) and distance > 8:
			animated_sprite_2d.flip_h = direction.x < 0
	position += velocity * delta

func _on_animation_finished() -> void:
	if state == State.ATTACK:
		_set_state(State.IDLE)
		attack_cooldown_remaining = 1

func _set_state(new_state: State) -> void:
	if state == new_state: return
	
	state = new_state
	animated_sprite_2d.play(State.keys()[state].to_lower())
