using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.Audio;
using UnityEngine;
using System.Linq;


/*
 * The following class is referencing Malcolm-Q's audio mixer fix
 MIT License

Copyright (c) 2023 Lethal Company Community

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */



namespace FuturamaItems.Misc
{
    public class AudioMixerFixer : MonoBehaviour // thanks Willis
    {
        private static AudioMixerGroup _masterDiageticMixer;
        public static AudioMixerGroup MasterDiageticMixer
        {
            get
            {
                if (_masterDiageticMixer == null)
                {
                    var referenceAudioSource = GameNetworkManager.Instance.GetComponent<NetworkManager>().NetworkConfig.Prefabs.Prefabs
                        .Select(p => p.Prefab.GetComponentInChildren<NoisemakerProp>())
                        .Where(p => p != null)
                        .Select(p => p.GetComponentInChildren<AudioSource>())
                        .Where(p => p != null)
                        .FirstOrDefault();
                    if (referenceAudioSource == null)
                    {
                        throw new Exception("Failed to locate a suitable AudioSource output mixer to reference! Could you be calling this method before the GameNetworkManager is initialized?");
                    }
                    _masterDiageticMixer = referenceAudioSource.outputAudioMixerGroup;
                }
                return _masterDiageticMixer;
            }
        }

        [SerializeField]
        private List<AudioSource> sourcesToFix;

        private void Start()
        {
            foreach (var source in GetComponentsInChildren<AudioSource>())
            {
                if (sourcesToFix.Contains(source)) continue;
                sourcesToFix.Add(source);
            }
            foreach (var source in sourcesToFix)
            {
                source.outputAudioMixerGroup = MasterDiageticMixer;
            }
        }
    }
}