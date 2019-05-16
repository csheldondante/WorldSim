using System.Collections.Generic;

namespace CSD{

	public interface IEntity<EV>{
		T GetComponent<T>() where T : IComponent<EV>;
		T GetComponent<T>(IComparer<T> comparator) where T : IComponent<EV>;
		List<T> GetComponents<T>() where T : IComponent<EV>;
		bool AddComponent<T> (T component) where T :IComponent<EV>;
		bool IsDestroyed();
		bool HasComponent<T> () where T : IComponent<EV>;
		void SetDestroyed(bool destroyed);
		EV GetView ();
		void SetView(EV view);
	}

	public interface IComponent<EV>{
		IEntity<EV> GetEntity();
		bool SetEntity(IEntity<EV> entity);
	}

	public interface IUpdatable{
		void Update (float deltaTime);
	}


	public class Entity<EV> : IEntity<EV> {
		private List<IComponent<EV>> components = new List<IComponent<EV>> ();
		private EV view;
		private bool isDestroyed = false;
		public bool IsDestroyed() {return isDestroyed;}
		public void SetDestroyed(bool destroyed) {isDestroyed = destroyed;}

		public Entity(){
		}

		public bool HasComponent<T>() where T : IComponent<EV>{
			return GetComponent<T> () != null;
		}

		public T GetComponent<T>() where T : IComponent<EV>{
			foreach (var component in components) {
				if (component.GetType () == typeof(T))
					return (T)component;
			}
			return default(T);
		}

		public T GetOrCreateComponent<T>() where T : IComponent<EV>, new() {
			foreach (var component in components) {
				if (component.GetType () == typeof(T))
					return (T)component;
			}
			if (typeof(T).IsSubclassOf(typeof(Component<EV>))) {
				var newComponent = new T();
				this.AddComponent(newComponent);
				return newComponent;
			}
			return default(T);
		}

		public T GetComponent<T>(IComparer<T> comparator) where T : IComponent<EV> {
			List<T> componentsOfType = GetComponents<T> ();
			if (componentsOfType.Count < 1)
				return default(T);
			componentsOfType.Sort (comparator);//can add util method to return max
			return componentsOfType[0];
		}

		public List<T> GetComponents<T>() where T : IComponent<EV> {
			List<T> components = new List<T> ();
			foreach (var component in components) {
				if (component.GetType () == typeof(T))
					components.Add ((T)component);
			}
			return components;
		}

		public bool AddComponent<T> (T component) where T :IComponent<EV>{
			if (component.GetEntity () != null)
				return false;
			component.SetEntity (this);
			components.Add (component);
			//TODO add stuff like checking preconditions for component
			return true;
		}

		public EV GetView(){
			return view;
		}

		public void SetView(EV view){
			this.view = view;
		}

	}

	public class Component<EV> : IComponent<EV>{
		private IEntity<EV> entity;
		public IEntity<EV> GetEntity (){
			return entity;
		}
		public bool SetEntity(IEntity<EV> entity){
			if (this.entity != null)
				return false;
			this.entity = entity;
			return true;
		}
		public class ComponentParams{
			public int randomSeed;
		}
		public Component (IEntity<EV> entity) {
			entity.AddComponent(this);
		}
		public Component () {}
	}

	public class UpdateableComponent<EV,WV> : Component<EV>, IUpdatable{
		public UpdateableComponent () {
		}

		public virtual void Update(float time){
		}

		public virtual double SecondsPerCall(){
			return 1f;
		}
	}

}
