
using System.Collections.Generic;

namespace CSD{

	public interface IViewComponent : IComponent {
	}

	public interface IWorldView<VC> where VC : IViewComponent {
		VC GetView(IEntity entity);
		float GetDistance(IEntity obj1, IEntity obj2);
		Objective GetRangeObjective(IEntity obj1, IEntity obj2);
		WorldEvent MoveTo(IEntity entity, IEntity target);
	}



	public class WorldSim<EV,WV> where WV : IWorldView<EV> where EV : IViewComponent{
		private static WorldSim<EV,WV> instance;
		private HashSet<IUpdatable> updatables = new HashSet<IUpdatable>();
		private HashSet<IEntity> entities = new HashSet<IEntity>();
		private WV worldView;
		public WorldSim(WV worldView){
			this.worldView = worldView;
			instance = this;
		}

		public Entity<EV> CreateEntity(){
			if (worldView == null)
				return null;
			Entity<EV> entity = new Entity<EV>();
			entity.SetView(worldView.GetView (entity));
			RegisterEntity (entity);
			return entity;
		}

		public static void RegisterUpdatable(IUpdatable updatable){
			if (instance == null || instance.updatables == null)
				return;
			instance.updatables.Add (updatable);
		}

		public static void RegisterEntity(IEntity entity){
			if (instance == null || instance.entities == null || instance.entities.Contains (entity))
				return;
			instance.entities.Add (entity);
		}

		public static float GetDistance(IEntity obj1, IEntity obj2){
			if (instance==null||instance.worldView == null)
				return float.NaN;
			return instance.worldView.GetDistance (obj1, obj2);
		}

		public void Tick(float deltaTime){
			//TODO sort these so they are stable for resolution purposes (requires duplicate list or different data structure)
			foreach (var updatable in updatables) {
				updatable.Update (deltaTime);
			}
		}
	}

}
