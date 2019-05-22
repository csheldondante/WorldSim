using System;
using System.Collections.Generic;

namespace CSD
{
	/*
	//TODO make resources use Requirements
	public class GetIntoRange<VC> : Objective {
		public float range;
		public IEntity<VC> target;
		public IEntity<VC> actor;
		public GetIntoRange (IEntity<VC> target, IEntity<VC> actor, float range){
			this.actor = actor;
			this.target = target;
			this.range = range;
		}
		public override bool IsComplete ()
		{
			if (target == null || actor == null || 0 <= range)
				return true;
			else
				return false;
		}
		public override WorldEvent GetWayToDo () {
			var actorAgent = actor.GetComponent<UpdateableComponent<VC>> ();
			if (actorAgent == null)
				return null;
			return null;//new MoveEvent (actorAgent, rangeTo.position, actorAgent.moveSpeed);
		}
	}//*/

	/*
	public class InstantEvent : WorldEvent
	{
		public override bool IsComplete() {
			return true;
		}
	}

	public class MoveEvent : WorldEvent
	{
		private AgentComponent mover;
		private PositionComponent moverPosition;
		private Vector2 desiredPosition;
		private float moveSpeed;
		private float progress;
		private float maxDist;

		public MoveEvent (AgentComponent mover, Vector2 desiredPosition, float moveSpeed)
		{
			this.mover = mover;
			this.moverPosition = mover.GetEntity ().GetComponent<PositionComponent> ();
			this.desiredPosition = desiredPosition;
			this.moveSpeed = moveSpeed;
			this.maxDist = Vector2.Distance (desiredPosition, moverPosition.position);
		}

		public override void Initialize ()
		{
			ProceduralWorldSimulator.RegisterUpdatable (this);
			mover.movement.user = this;
			this.maxDist = Vector2.Distance (desiredPosition, moverPosition.position);
			Activate ();
		}

		public override List<Resource> GetRequiredResources(){
			List<Resource> resources = new List<Resource> ();
			resources.Add (mover.movement);
			return resources;
		}

		public override string GetName(){

			return "Move to ("+desiredPosition.x+","+desiredPosition.y+")";
		}

		public override string GetDescription(){
			return mover.name+" is moving to ("+desiredPosition.x+","+desiredPosition.y+")";
		}

		//return true when the event is complete and is ready to be destroyed
		public override void Update(float time){
			if (mover == null || progress >= 1.0f || mover.movement.user != this)
				progress = 1.0f;
			else {
				Vector2 desiredDelta = desiredPosition - moverPosition.position;
				if (desiredDelta.magnitude > moveSpeed * time) {
					desiredDelta.Normalize ();
					desiredDelta *= moveSpeed * time;
				} else {
					moverPosition.position = desiredPosition;
					progress = 1.0f;
					return;
				}
				moverPosition.position += desiredDelta;
				float distance = Vector2.Distance (moverPosition.position, desiredPosition);
				progress = (maxDist-distance)/maxDist;
			}
			// Move whatever they're hauling.
			var inventoryComponent = mover.GetEntity().GetComponent<InventoryComponent> ();
			if (!inventoryComponent.haulingSlot.IsFree()) {
				var hauledPosition = inventoryComponent.haulingSlot.item.GetEntity().GetComponent<PositionComponent> ();
				hauledPosition.position = moverPosition.position + Vector2.up;
			}
			if (progress >= 1.0f)
				Debug.Log ("Finished " + GetName ());
		}

		public override bool IsComplete(){
			return progress >= 1.0f;
		}
	}

	public class EatEvent : WorldEvent
	{
		private AgentComponent eater;
		private PositionComponent food;
		private PlantComponent plant;
		private float progress;
		private float initialSize;

		public EatEvent (IEntity eater, PositionComponent food)
		{
			this.eater = eater.GetComponent<AgentComponent>();
			this.food = food;
			this.plant = food.GetEntity ().GetComponent<PlantComponent> ();
		}

		public override void Initialize ()
		{
			progress = 0f;
			eater.movement.user = this;
			Activate ();
			initialSize = plant.size;
		}

		public override List<Resource> GetRequiredResources(){
			//TODO get requirements instead of just resources so it can be used by the AI to plan out actions
			List<Resource> resources = new List<Resource> ();
			resources.Add (eater.movement);
			resources.Add (plant.substance);
			return resources;
		}

		public override string GetName(){
			return "Eat food";
		}

		public override string GetDescription(){
			return eater.name+" is eating food";
		}

		//return true when the event is complete and is ready to be destroyed
		public override void Update(float time){
			if (eater == null || food == null || progress >= 1.0f || eater.movement.user != this || plant == null || plant.substance.user != this)
				progress = 1.0f;
			else {
				plant.size -= eater.eatSpeed*time;
				progress = 1- plant.size / initialSize;
			}
			if (IsComplete()) {
				food.GetEntity().SetDestroyed(true);
			}
			if (progress >= 1.0f)
				Debug.Log ("Finished " + GetName ());
		}

		public override bool IsComplete(){
			return progress >= 1.0f;
		}

		public override List<Requirement> GetRequirments(){
			List<Requirement> requirements = new List<Requirement> ();
			requirements.Add (new RangeRequirement (food, eater.GetEntity().GetComponent<PositionComponent>(), .1f));
			return requirements;
		}
	}
	//*/
}
