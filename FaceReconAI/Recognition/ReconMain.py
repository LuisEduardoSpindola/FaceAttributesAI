# -*- coding: utf-8 -*-

import cv2
from deepface import DeepFace
import sys
import json

# Receber o caminho da imagem como argumento
image_path = sys.argv[1]

# Realizar a análise facial
img = cv2.imread(image_path)
result = DeepFace.analyze(img, actions=('age', 'emotion'))

# Imprimir o resultado JSON na saída padrão
print(json.dumps(result))
