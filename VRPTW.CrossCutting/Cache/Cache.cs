using System;
using System.Runtime.Caching;
using VRPTW.CrossCutting.Configuration;

namespace VRPTW.CrossCutting.Cache
{
	public static class Cache
	{
		public static T GetItemFromCache<T>(string id, Func<T> metodo)
		{
			MemoryCache cache = MemoryCache.Default;

			object objeto = cache.Get(id);

			if (objeto == null)
			{
				objeto = metodo.DynamicInvoke();
				if (objeto != null && GeneralConfigurations.CACHE_DURATION_IN_MINUTES > 0)
					cache.Add(id, objeto, DateTime.Now.AddMinutes(GeneralConfigurations.CACHE_DURATION_IN_MINUTES));
			}

			return (T)objeto;
		}

		public static T GetItemFromCache<T, T1>(string id, Func<T1, T> metodo, T1 parametro, int? tempoCache = null)
		{
			MemoryCache cache = MemoryCache.Default;

			id = string.Format("{0}_{1}", id, parametro);

			object objeto = cache.Get(id);

			if (objeto == null)
			{
				objeto = metodo.DynamicInvoke(parametro);
				if (objeto != null && GeneralConfigurations.CACHE_DURATION_IN_MINUTES > 0)
					cache.Add(id, objeto, DateTime.Now.AddMinutes(tempoCache ?? GeneralConfigurations.CACHE_DURATION_IN_MINUTES));
			}

			return (T)objeto;

		}

		public static T GetItemFromCache<T, T1, T2>(string id, Func<T1, T2, T> metodo, T1 parametro1, T2 parametro2)
		{
			MemoryCache cache = MemoryCache.Default;

			id = string.Format("{0}_{1}_{2}", id, parametro1, parametro2);

			object objeto = cache.Get(id);

			if (objeto == null)
			{
				objeto = metodo.DynamicInvoke(parametro1, parametro2);
				if (objeto != null && GeneralConfigurations.CACHE_DURATION_IN_MINUTES > 0)
					cache.Add(id, objeto, DateTime.Now.AddMinutes(GeneralConfigurations.CACHE_DURATION_IN_MINUTES));
			}

			return (T)objeto;
		}

		public static T GetItemFromCache<T, T1, T2, T3>(string id, Func<T1, T2, T3, T> metodo, T1 parametro1, T2 parametro2, T3 parametro3)
		{
			MemoryCache cache = MemoryCache.Default;

			id = string.Format("{0}_{1}_{2}_{3}", id, parametro1, parametro2, parametro3);

			object objeto = cache.Get(id);

			if (objeto == null)
			{
				objeto = metodo.DynamicInvoke(parametro1, parametro2, parametro3);
				if (objeto != null && GeneralConfigurations.CACHE_DURATION_IN_MINUTES > 0)
					cache.Add(id, objeto, DateTime.Now.AddMinutes(GeneralConfigurations.CACHE_DURATION_IN_MINUTES));
			}

			return (T)objeto;
		}

		public static T GetItemFromCache<T, T1, T2, T3, T4>(string id, Func<T1, T2, T3, T4, T> metodo, T1 parametro1, T2 parametro2, T3 parametro3, T4 parametro4)
		{
			MemoryCache cache = MemoryCache.Default;

			id = string.Format("{0}_{1}_{2}_{3}_{4}", id, parametro1, parametro2, parametro3, parametro4);

			object objeto = cache.Get(id);

			if (objeto == null)
			{
				objeto = metodo.DynamicInvoke(parametro1, parametro2, parametro3, parametro4);
				if (objeto != null && GeneralConfigurations.CACHE_DURATION_IN_MINUTES > 0)
					cache.Add(id, objeto, DateTime.Now.AddMinutes(GeneralConfigurations.CACHE_DURATION_IN_MINUTES));
			}

			return (T)objeto;
		}

		public static void RemoveItemFromCache(string id)
		{
			MemoryCache cache = MemoryCache.Default;

			object objeto = cache.Get(id);
			if (objeto != null)
				cache.Remove(id);
		}
	}
}
