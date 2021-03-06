﻿using System.Collections.Generic;

namespace CSD{

	public interface IEntity{
		T GetComponent<T>() where T : IComponent;
		T GetComponent<T>(IComparer<T> comparator) where T : IComponent;
		List<T> GetComponents<T>() where T : IComponent;
		bool AddComponent<T> (T component) where T :IComponent;
		bool IsDestroyed();
		bool HasComponent<T> () where T : IComponent;
		void SetDestroyed(bool destroyed);
	}

	public interface IComponent{
		IEntity GetEntity();
		bool SetEntity(IEntity entity);
	}

	public interface IUpdatable{
		void Update (float deltaTime);
	}


	public class Entity<EV> : IEntity {
		private List<IComponent> components = new List<IComponent> ();
		private EV view;
		private bool isDestroyed = false;
		public bool IsDestroyed() {return isDestroyed;}
		public void SetDestroyed(bool destroyed) {isDestroyed = destroyed;}

		public Entity(){
		}

		public bool HasComponent<T>() where T : IComponent{
			return GetComponent<T> () != null;
		}

		public T GetComponent<T>() where T : IComponent{
			foreach (var component in components) {
				if (component.GetType () == typeof(T))
					return (T)component;
			}
			return default(T);
		}

		public T GetOrCreateComponent<T>() where T : IComponent, new() {
			foreach (var component in components) {
				if (component.GetType () == typeof(T))
					return (T)component;
			}
			if (typeof(T).IsSubclassOf(typeof(Component))) {
				var newComponent = new T();
				this.AddComponent(newComponent);
				return newComponent;
			}
			return default(T);
		}

		public T GetComponent<T>(IComparer<T> comparator) where T : IComponent {
			List<T> componentsOfType = GetComponents<T> ();
			if (componentsOfType.Count < 1)
				return default(T);
			componentsOfType.Sort (comparator);//can add util method to return max
			return componentsOfType[0];
		}

		public List<T> GetComponents<T>() where T : IComponent {
			List<T> components = new List<T> ();
			foreach (var component in components) {
				if (component.GetType () == typeof(T))
					components.Add ((T)component);
			}
			return components;
		}

		public bool AddComponent<T> (T component) where T :IComponent{
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

	public class Component : IComponent{
		private IEntity entity;
		public IEntity GetEntity (){
			return entity;
		}
		public bool SetEntity(IEntity entity){
			if (this.entity != null)
				return false;
			this.entity = entity;
			return true;
		}
		public class ComponentParams{
			public int randomSeed;
		}
		public Component (IEntity entity) {
			entity.AddComponent(this);
		}
		public Component () {}
	}

	public class UpdateableComponent : Component, IUpdatable{
		public UpdateableComponent () {
		}

		public virtual void Update(float time){
		}

		public virtual double SecondsPerCall(){
			return 1f;
		}
	}

}
