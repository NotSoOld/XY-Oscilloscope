using UnityEngine;
using System.Collections;

public class Oscillator : MonoBehaviour
{
	
	private AudioSource audioSrc;
	public AudioClip clip;
	private Transform painter;
	private LineRenderer painterLine;


	private void Start()
	{
		audioSrc = GetComponent<AudioSource>();
		painter = this.transform;
		painterLine = painter.GetComponent<LineRenderer>();

		StartCoroutine(Osc(clip.frequency));
	}


	/// Set physics tick time = 0.000125 (8 KHz! O.o).
	private IEnumerator Osc(int inputFrequency)
	{
		audioSrc.clip = clip;
		yield return new WaitForSeconds(1f);
		audioSrc.Play();

		/// For any audio.
		int i = 0;				/// (Start position in (samples * factor) can be set here.)
		int factor = (int)(inputFrequency * 0.008f);
		int bufferSize = 4;
		float[] samples = new float[factor];
		painterLine.numPositions = (int)((bufferSize + 0.5f) * factor);
		Vector3[] prevLine = new Vector3[bufferSize * factor];
		while(i < (clip.samples / factor))
		{
			clip.GetData(samples, i * factor);
			painter.position = new Vector3(samples[0], samples[1]) * 5f;
			painterLine.SetPositions(prevLine);
			for(int j = factor / 2; j < bufferSize * factor; j++)
			{
				prevLine[j - factor / 2] = prevLine[j];
			}
			for(int j = 0; j < factor; j += 2)
			{
				painterLine.SetPosition((int)(j / 2 + bufferSize * factor), new Vector3(samples[j], samples[j+1]) * 5f);
				prevLine[(int)(j / 2 + (bufferSize - 0.5f) * factor)] = new Vector3(samples[j], samples[j+1]) * 5f;
			}
			yield return new WaitForFixedUpdate();
			i++;
		}

	}

}