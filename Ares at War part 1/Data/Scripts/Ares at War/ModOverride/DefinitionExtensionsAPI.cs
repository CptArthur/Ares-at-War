using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using IMyTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;

namespace AresAtWar.BlockExtensionsAPI
{

	public class DefinitionExtensionsAPI
	{

		//create this class object asap in your mod before loaddata executes. 
		public long MODID = 2756894170;


		IReadOnlyDictionary<Type, Delegate> _methods;
		Action m_callback = null;
		bool m_init = false;
		/// <summary>
		/// Indicates if the API has successfully init, inits during SessionComponent::LoadData
		/// </summary>
		public bool Init
		{
			get
			{
				return m_init;
			}
		}

		/// <summary>
		/// Init in your constructor of a session component, BlockExtensions is initialized in LoadData and will call the callback when initialized. Note, make sure to call UnloadData when session completes. 
		/// </summary>
		/// <param name="callback">Callback action when init</param>
		public DefinitionExtensionsAPI(Action callback)
		{
			if (MyAPIGateway.Utilities == null)
				MyAPIGateway.Utilities = MyAPIUtilities.Static;

			m_callback = callback;

			MyAPIGateway.Utilities.RegisterMessageHandler(MODID, recieveModHandlers);
		}
		/// <summary>
		/// Call when your mod is unloaded
		/// </summary>
		public void UnloadData()
		{
			MyAPIGateway.Utilities.UnregisterMessageHandler(MODID, recieveModHandlers);
		}

		public enum AdditionalMethods : int
		{
			None=0,
			RegisterTSS=1,
			UnRegisterTSS=2,
			RegisterDataTSS = 3,
			GetDataTSS = 4,
			DefIDExists = 5,
			GetGroups = 6,
			GetProperties = 7
		}
		private void recieveModHandlers(object obj)
		{
			if (Init)
				return;
			if(obj is IReadOnlyDictionary<Type, Delegate>)
			{
				_methods = (IReadOnlyDictionary<Type, Delegate>)obj;
				Assign(typeof(float), ref _floatMethod);
				Assign(typeof(double), ref _doubleMethod);
				Assign(typeof(int), ref _intMethod);
				Assign(typeof(long), ref _longMethod);
				Assign(typeof(string), ref _textMethod);
				Assign(typeof(Color), ref _colorMethod);
				Assign(typeof(bool), ref _booleanMethod);
				Assign(typeof(Vector2I), ref _vector2IMethod);
				Assign(typeof(Vector2D), ref _vector2DMethod);
				Assign(typeof(Vector3I), ref _vector3IMethod);
				Assign(typeof(Vector3D), ref _vector3DMethod);
				Assign(typeof(MyGameLogicComponent), ref _setGameLogic);
				Assign(typeof(Delegate), ref _getDelegate);
				if(_getDelegate != null)
				{
					Assign(AdditionalMethods.RegisterTSS, ref _RegisterTSS);
					Assign(AdditionalMethods.UnRegisterTSS, ref _UnregisterTSS);
					Assign(AdditionalMethods.RegisterDataTSS, ref _RegisterTSSDataComponent);
					Assign(AdditionalMethods.GetDataTSS, ref _GetTSSDataComponent);
					Assign(AdditionalMethods.DefIDExists, ref _DefIDExists);
					Assign(AdditionalMethods.GetGroups, ref _GetGroups);
					Assign(AdditionalMethods.GetProperties, ref _GetProperties);
				}
				m_init = true;
				m_callback?.Invoke();
			}
		}

		private void Assign<T>(Type valuetype, ref T method) where T : class
		{
			method = _methods[valuetype] as T;

		}
		private void Assign<T>(AdditionalMethods mt, ref T method) where T : class
		{
			method = _getDelegate((int)mt) as T;
		}
			

		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, float>> _floatMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, double>> _doubleMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, int>> _intMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, long>> _longMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, string>> _textMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, Color>> _colorMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, bool>> _booleanMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, Vector2I>> _vector2IMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, Vector2D>> _vector2DMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, Vector3I>> _vector3IMethod;
		private Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, Vector3D>> _vector3DMethod;
		private Action<MyStringId, IMyModContext, Func<MyGameLogicComponent>> _setGameLogic;
		private Func<int, Delegate> _getDelegate;
		private Action<MyTSSCommon, IMyTextSurface, IMyTerminalBlock, Action<MyTSSCommon, IMyTerminalBlock, List<IMyTerminalControl>>> _RegisterTSS;
		private Action<MyTSSCommon, IMyTextSurface, IMyTerminalBlock> _UnregisterTSS;
		private Action<Type, Type, IMyModContext, Func<MyEntityComponentBase>> _RegisterTSSDataComponent;
		private Func<Type, IMyTerminalBlock, MyEntityComponentBase> _GetTSSDataComponent;
		private Func<MyDefinitionId, bool> _DefIDExists;
		private Action<MyDefinitionId, List<MyStringId>> _GetGroups;
		private Action<MyDefinitionId, MyStringId, List<MyTuple<MyStringId, Type>>> _GetProperties;

		/// <summary>
		/// Checks if definition has any defined groups
		/// </summary>
		/// <param name="definition">Definition to check</param>
		/// <returns>result</returns>
		public bool DefinitionIdExists(MyDefinitionId definition)
		{
			return _DefIDExists?.Invoke(definition) ?? false;
		}

		/// <summary>
		/// Gets all groups assigned to a specific definition id
		/// </summary>
		/// <param name="definition">DefinitionId to fetch groups</param>
		/// <param name="grouplist">List groups will be added to. Not cleared internally. Will not allocate a new list if null.</param>
		public void GetGroups(MyDefinitionId definition, List<MyStringId> grouplist)
		{
			_GetGroups?.Invoke(definition, grouplist);
		}

		/// <summary>
		/// Gets all properties assigned to a block and group
		/// </summary>
		/// <param name="definition">Definition ID to fetch properties</param>
		/// <param name="group">Group to fetch properties</param>
		/// <param name="properties">List of properties and type. Not cleared internally. Will not allocate a new list if null.</param>
		public void GetProperties(MyDefinitionId definition, MyStringId groupid, List<MyTuple<MyStringId, Type>> properties)
		{
			_GetProperties?.Invoke(definition, groupid, properties);
		}


		/// <summary>
		/// Must be called in the scripts constructor. 
		/// </summary>
		/// <param name="script">pass in the calling script (this)</param>
		/// <param name="surface">IMyTextSurface the script belongs to</param>
		/// <param name="block">Block the surface blongs to</param>
		/// <param name="controlgetter">Custom Control Getter</param>
		public void RegisterTSS(MyTSSCommon script, IMyTextSurface surface, IMyTerminalBlock block, Action<MyTSSCommon, IMyTerminalBlock, List<IMyTerminalControl>> controlgetter)
		{
			_RegisterTSS?.Invoke(script, surface, block, controlgetter);
		}

		/// <summary>
		/// Must be called in the scripts Dispose method
		/// </summary>
		/// <param name="script">pass in the calling script (this)</param>
		/// <param name="surface">IMyTextSurface the script belongs to</param>
		/// <param name="block">block the surface belongs to</param>
		public void UnRegisterTSS(MyTSSCommon script, IMyTextSurface surface, IMyTerminalBlock block)
		{
			_UnregisterTSS?.Invoke(script, surface, block);
		}

		/// <summary>
		/// Automatically creates and registers a MyEntityComponentBase when TSS Script is registered.
		/// </summary>
		/// <typeparam name="T">TSS Script</typeparam>
		/// <typeparam name="U">Entity Component to automatically load when TSS script registers</typeparam>
		/// <param name="modContext">Pass in ModContext from your session component</param>
		/// <param name="customFactory">Optional, must return your MyEntityComponentBase, for example if you want to create an object pool instead of creating a new object</param>
		public void RegisterTSSDataComponent<T, U>(IMyModContext modContext, Func<U> customFactory = null) where U : MyEntityComponentBase, new()
			where T : MyTSSCommon
		{
			if (customFactory == null)
				customFactory = () => { return new U(); };

			_RegisterTSSDataComponent?.Invoke(typeof(T), typeof(U), modContext, customFactory);
		}

		/// <summary>
		/// Grabs the registered TSS Data Component for the block, alternatively you can just fetch it directly via block.Components.Get<>
		/// </summary>
		/// <typeparam name="T">TSS Script requesting its data component</typeparam>
		/// <param name="block"></param>
		/// <returns></returns>
		public MyEntityComponentBase GetTSSDataComponent<T>(IMyTerminalBlock block) where T : MyTSSCommon
		{
			return _GetTSSDataComponent(typeof(T), block);
		}

		/// Registers a GameLogic class in ModExtensions, will be applied to all blocks that define the component tag in definition. Note: ModContext is null for GameLogic inited in this way
		/// </summary>
		/// <typeparam name="T">Your GameLogicComponent</typeparam>
		/// <param name="componentName">name of the component in the SBC file to attach to</param>
		/// <param name="mod">pass your mod context</param>
		/// <param name="customFactory">Optional, custom factory delegate must return your gamelogic component</param>
		public void RegisterGameLogic<T>(MyStringId componentName, IMyModContext mod, Func<T> customFactory = null) where T : MyGameLogicComponent, new()
		{
			if(customFactory == null)
				customFactory = () => { return new T(); };

			_setGameLogic?.Invoke(componentName, mod, customFactory);
		}

		/// <summary>
		/// Gets text block property
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetText(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out string value)
		{
			var retval = _textMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Alias for TryGetText
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetString(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out string value)
		{
			return TryGetText(definition, group, propertyname, out value);
		}

		/// <summary>
		/// Gets integer block property, note integer value is stored as a long and is cast to int. 
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetInt(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out int value)
		{
			var retval = _intMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets integer block property.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetLong(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out long value)
		{
			var retval = _longMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets floating precision block property. Note, backend value is stored as a double and cast to float.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetFloat(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out float value)
		{
			var retval = _floatMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets double floating precision block property.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetDouble(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out double value)
		{
			var retval = _doubleMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets boolean block property.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetBool(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out bool value)
		{
			var retval = _booleanMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets color block property.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetColor(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out Color value)
		{
			var retval = _colorMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets Vector2I (integer) block property.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetVector2I(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out Vector2I value)
		{
			var retval = _vector2IMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets Vector2D (double) block property.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetVector2D(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out Vector2D value)
		{
			var retval = _vector2DMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets Vector3I (integer) block property.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetVector3I(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out Vector3I value)
		{
			var retval = _vector3IMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Gets Vector3D (double) block property.
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGetVector3D(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out Vector3D value)
		{
			var retval = _vector3DMethod.Invoke(definition, group, propertyname);
			value = retval.Item2;
			return retval.Item1;
		}

		/// <summary>
		/// Generic to expand supported variables in the future can also be used for existing types. 
		/// </summary>
		/// <param name="definition">MyDefinitionId that is being extended</param>
		/// <param name="group">ModExtensions Group</param>
		/// <param name="propertyname">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <returns>Success value, false indicates property does not exist</returns>
		public bool TryGet<T>(MyDefinitionId definition, MyStringId group, MyStringId propertyname, out T value)
		{
			if(!_methods?.ContainsKey(typeof(T)) ?? false)
			{
				value = default(T);
				return false;
			}
			var result = ((Func<MyDefinitionId, MyStringId, MyStringId, MyTuple<bool, T>>)_methods[typeof(T)]).Invoke(definition, group, propertyname);
			value = result.Item2;
			return result.Item1;
		}
	}
}
