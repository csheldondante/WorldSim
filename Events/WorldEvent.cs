using System.Collections.Generic;

namespace CSD{
	public class Objective {
		public virtual bool IsComplete(){
			return false;
		}
		public virtual WorldEvent GetWayToDo () {
			return null;
		}
	}

	public interface IResourceHolder{
		List<Resource> GetRequiredResources();
		List<Objective> GetRequirements();
	}

	public class WorldEvent : IUpdatable, IResourceHolder
	{
		public WorldEvent() {
		}

		public virtual List<Resource> GetRequiredResources(){
			return new List<Resource>();
		}

		public virtual void Update(float time){
		}

		public virtual bool IsComplete(){
			return false;
		}

		public virtual List<Objective> GetRequirements(){
			return null;
		}
	}
}