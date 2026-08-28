class_name Player
extends CharacterBody2D

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

const SPEED: float = 300.0
const ACCELERATION: float = SPEED * 3

enum State {IDLE, RUN, ATTACK1, ATTACK2, GUARD}
var state: State = State.IDLE

var attack2_queued: bool = false
var attack1_active_time: float = 0.0
var attack1_cooldown_remaining: float = 0.0

func _ready() -> void:
	animated_sprite_2d.animation_finished.connect(_on_animation_finished)

func _physics_process(delta: float) -> void:
	_update_attack_timers(delta)
	_handle_attack_input()
	_handle_movement(delta)
	move_and_slide()

func _update_attack_timers(delta: float) -> void:
	attack1_cooldown_remaining = max(0.0, attack1_cooldown_remaining - delta)

	if state == State.ATTACK1:
		attack1_active_time += delta

func _handle_attack_input() -> void:
	if !Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT): return

	if state == State.ATTACK1:
		if attack1_active_time >= 0.15:
			attack2_queued = true
	elif state in [State.IDLE, State.RUN] and attack1_cooldown_remaining <= 0.0:
		_set_state(State.ATTACK1)

func _handle_movement(delta: float) -> void:
	var input_direction: Vector2 = Input.get_vector("left", "right", "up", "down")
	var target_velocity: Vector2 = input_direction * SPEED
	velocity = velocity.move_toward(target_velocity, ACCELERATION * delta)

	if state in [State.IDLE, State.RUN]:
		_set_state(State.IDLE if velocity.is_zero_approx() else State.RUN)

	if !is_zero_approx(velocity.x):
		animated_sprite_2d.flip_h = velocity.x < 0

func _on_animation_finished() -> void:
	if state != State.ATTACK1:
		_set_state(State.IDLE)
		return

	attack1_cooldown_remaining = 0.8

	if attack2_queued:
		attack2_queued = false
		_set_state(State.ATTACK2)
	else:
		_set_state(State.IDLE)

func _set_state(new_state: State) -> void:
	if state == new_state: return

	state = new_state

	if state == State.ATTACK1:
		attack1_active_time = 0.0
		attack2_queued = false

	animated_sprite_2d.play(State.keys()[state].to_lower())
